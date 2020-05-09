using SatisfactorySaveParser.Save;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SatisfactorySaveEditor.Service.Undo
{
    public class TransactionalFGSaveSession
    {
        private readonly FGSaveSession _session;

        private readonly List<SaveObject> _cachedObjects = new List<SaveObject>();
        private readonly List<SaveObject> _addedObjects = new List<SaveObject>();
        private readonly List<SaveObject> _removedObjects = new List<SaveObject>();

        public bool IsActive { get; private set; } = true;
        public string Name { get; }

        public TransactionalFGSaveSession(string name, FGSaveSession session)
        {
            Name = name;
            _session = session;
        }

        public List<SaveObject> AccessObjects(Func<SaveObject, bool> predicate)
        {
            if (!IsActive) throw new InvalidOperationException("Cannot change a finished transaction");

            var results = _session.Objects.Where(predicate).ToList();

            // TODO: Implement
            //foreach (var result in results) _cachedObjects.Add(result.Clone());

            return results;
        }

        /// <summary>
        /// Returns objects matching a predicate. I cannot actually stop you from writing, so any changes you make to these objects won't be added to the Undo system.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<SaveObject> AccessObjectsReadOnly(Func<SaveObject, bool> predicate)
        {
            if (!IsActive) throw new InvalidOperationException("Cannot read data from a finished transaction as it wouldn't reflect the current state of data");

            return _session.Objects.Where(predicate);
        }

        public void RemoveObject(SaveObject obj)
        {
            if (!IsActive) throw new InvalidOperationException("Cannot change a finished transaction");

            /*
            var removed = _session.RemoveObject(obj);
            foreach (var rem in removed) _removedObjects.Add(rem);
            */
        }

        public void RemoveObject(string instance)
        {
            if (!IsActive) throw new InvalidOperationException("Cannot change a finished transaction");

            /*
            var removed = _session.RemoveObject(instance);
            foreach (var rem in removed) _removedObjects.Add(rem);
            */
        }

        public SaveObject AddObject(string type)
        {
            if (!IsActive) throw new InvalidOperationException("Cannot change a finished transaction");

            var obj = _session.AddObject(type);
            _addedObjects.Add(obj);

            return obj;
        }

        public T AddObject<T>() where T : SaveObject
        {
            if (!IsActive) throw new InvalidOperationException("Cannot change a finished transaction");

            var obj = _session.AddObject<T>();
            _addedObjects.Add(obj);

            return obj;
        }

        public void Undo()
        {
            if (IsActive) throw new InvalidOperationException("Cannot undo an unfinished transaction");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Finishes the current transaction, preventing any further changes
        /// </summary>
        public void Finish()
        {
            if (!IsActive) throw new InvalidOperationException("Cannot finish an already finished transaction");

            IsActive = false;
        }
    }
}
