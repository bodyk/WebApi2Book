using System.Linq;
using NHibernate;
using WebApi2Book.Common;
using WebApi2Book.Common.Security;
using WebApi2Book.Data.Entities;
using WebApi2Book.Data.Exceptions;

namespace WebApi2Book.Data.QueryProcessors
{
    public class AddTaskQueryProcessor : IAddTaskQueryProcessor
    {
        private readonly IDateTime _dateTime;
        private readonly ISession _session;
        private readonly IUserSession _userSession;

        public AddTaskQueryProcessor(ISession session, IUserSession userSession, IDateTime dateTime)
        {
            _session = session;
            _userSession = userSession;
            _dateTime = dateTime;
        }

        public void AddTask(Task task)
        {
            task.CreatedDate = _dateTime.UtcNow;
            task.Status = _session.QueryOver<Status>()
                .Where(x => x.Name == "Not Started")
                .SingleOrDefault();
            //task.CreatedBy = _session.QueryOver<User>()
            //    .Where(x => x.Username == _userSession.UserName)
            //    .SingleOrDefault();

            task.CreatedBy = _session.Get<User>(1L);

            if (task.Users == null || !task.Users.Any())
                return;

            for (var i = 0; i < task.Users.Count; ++i)
            {
                var user = task.Users[i];
                var persistedUser = _session.Get<User>(user.UserId);
                task.Users[i] = persistedUser 
                    ?? throw new ChildObjectNotFoundException("User not found");
            }

            _session.SaveOrUpdate(task);
        }
    }
}
