using System;
using System.Collections.Generic;
using System.Linq;
// GB classes
using GB.SUTEL.Shared;
using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.DAL
{
    /// <summary>
    /// Just for explanation, not for production
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    public class Maintainable1 : LocalContextualizer, IMaintainable<Web_SampleEntity>
    {
        public Maintainable1(ApplicationContext appContext) : base(appContext) { }
        /// <summary>
        /// As you may suspect, you should not trust this collection
        /// 'cause you know... it's fake
        /// </summary>
        List<Web_SampleEntity> doNotTrustThis = new List<Web_SampleEntity>() { 
            new Web_SampleEntity(){ Number = 1, Name = "foo"},
            new Web_SampleEntity(){ Number= 2, Name = "foo2"}
        };

        public Web_SampleEntity Single(System.Linq.Expressions.Expression<System.Func<Web_SampleEntity, bool>> expression)
        {
            try
            {
                throw new Exception();
                return doNotTrustThis.AsQueryable().Where(expression).SingleOrDefault();
            }
            catch (Exception ex)
            {
                // wow such exception, too easy, much coolness
                // just in case the exceptions was manually thrown from the previous code block
                if (ex is DataAccessException) throw;
                // as we create a DataAccessException Object, it is registered whenever it's defined
                // remember to change the "custom message"
                // here we are not saying the application anything else than an error message, 
                // yet it will register everything we need...!!!
                throw AppContext.ExceptionBuilder.BuildException("Custom Message", ex);
            }
        }

        public System.Linq.IQueryable All()
        {
            throw new System.NotImplementedException();
        }

        public void Add(Web_SampleEntity item)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Web_SampleEntity item)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Web_SampleEntity item)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(System.Linq.Expressions.Expression<System.Func<Web_SampleEntity, bool>> expression)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new System.NotImplementedException();
        }

        public void CommitChanges()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void g() {
            using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
            { 
            
            }
        }
    }
}
