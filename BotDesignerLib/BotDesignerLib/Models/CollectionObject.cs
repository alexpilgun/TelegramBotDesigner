using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BotDesignerLib
{
    [Serializable]
    public abstract class GuidEntity
    {
        public Guid Id { get; }

        public GuidEntity()
        {
            Id = Guid.NewGuid();
        }

        //public abstract void AddOrEdit(GuidEntity e);
    }

    [Serializable]
    public class CollectionObject<T> where T : GuidEntity
    {
        public T CurrentObject { get; set; }
        public List<T> Objects { get; }


        public CollectionObject()
        {
            Objects = new List<T>();
        }


        public void AddOrEdit(T obj)
        {
            var existingObject = Objects.Where(o => o.Id == obj.Id).FirstOrDefault();

            if (existingObject == default(T))
            {
                Objects.Add(obj);
            }
            else
            {
                var index = Objects.IndexOf(existingObject);
                Objects[index] = obj;
            }
        }

        public void ClearCurrentObject()
        {
            this.CurrentObject = null;
            return;
        }

        public void Delete(T obj)
        {
            if (obj != default(T))
            {
                Objects.Remove(obj);
            }
        }
    }
}
