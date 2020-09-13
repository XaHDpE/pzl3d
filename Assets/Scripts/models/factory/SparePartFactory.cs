using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using models.sp;
using UnityEngine;

namespace models.factory
{
    public class SparePartFactory
    {
        private MonoBehaviour _prefab;
        
        private Dictionary<string, Type> SparePartBaseByName;

        public SparePartFactory()
        {
            var spTypes = Assembly.GetAssembly(typeof(SparePartBase)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(SparePartBase)));
            
            SparePartBaseByName = new Dictionary<string, Type>();
            
            // populate dictionary
            foreach (var spType in spTypes)
            {
                var tempSp = Activator.CreateInstance(spType,
                    Vector3.zero, Quaternion.identity, 0, 0
                    ) as SparePartBase;
                SparePartBaseByName.Add(tempSp.name, spType);
            }
        } 
        
        public SparePartBase GetSparePartBase(string spType)
        {
            if (!SparePartBaseByName.ContainsKey(spType)) return null;
            var type = SparePartBaseByName[spType];
            var sp = Activator.CreateInstance( type,
                Vector3.zero, Quaternion.identity, 0, 0
                ) as SparePartBase;
            return sp;
        }
        
        internal IEnumerable<string> GetSparePartTypeNames()
        {
            return SparePartBaseByName.Keys;
        }
    }

    public static class SparePartStaticFactory
    {
        private static Dictionary<string, Type> _spByName;
        private static bool IsInitialized => _spByName != null;

        private static void InitializeFactory()
        {
            if (IsInitialized) return;
            
            var spTypes = Assembly.GetAssembly(typeof(SparePartBase)).GetTypes()
                .Where(typ => typ.IsClass && !typ.IsAbstract && typ.IsSubclassOf(typeof(SparePartBase)));
            
            _spByName = new Dictionary<string, Type>();
            
            // populate dictionary
            foreach (var spType in spTypes)
            {
                // Debug.Log(SparePartBaseType);
                var tempSp = Activator.CreateInstance(spType, 
                    Vector3.zero, Quaternion.identity, 0, 0) as SparePartBase;
                _spByName.Add(tempSp.name, spType);
            }
            
        } 
        
        public static SparePartBase GetSparePartBase(string spType)
        {
            InitializeFactory();
            if (!_spByName.ContainsKey(spType)) return null;
            var type = _spByName[spType];

            var sp = Activator.CreateInstance( type,
                new object[] { Vector3.zero, Quaternion.identity, 0, 0 }
            ) as SparePartBase;

            return sp;
        }
        
        internal static IEnumerable<string> GetSparePartBaseTypeNames()
        {
            InitializeFactory();
            return _spByName.Keys;
        }
        
    }
    
}