using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BotDesignerLib
{
    public interface IDataContext
    {

    }

    public abstract class GuidEntity
    {
        public Guid Id { get; }

        public GuidEntity()
        {
            Id = new Guid();
        }

        //public abstract void AddOrEdit(GuidEntity e);
    }

    public class CollectionObject<T> where T: GuidEntity
    {
        public T CurrentObject { get; set; }
        public List<T> Objects { get; }

        
        public CollectionObject ()
        {
            Objects = new List<T>();
        }


        public void AddOrEdit(T obj)
        {
            var existingObject = Objects.Where(o => o.Id == obj.Id).FirstOrDefault();

            if (existingObject == default(T) )
            {
                Objects.Add(obj);
            }
            else
            {
                existingObject = obj;
            }
        }

        public void ClearCurrentObject()
        {
            this.CurrentObject = null;
            return;
        }
    }
}
