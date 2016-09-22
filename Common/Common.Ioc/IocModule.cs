using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BLToolkit.Reflection;
using Ninject.Modules;

namespace Common.Ioc
{
    public class IocModule : NinjectModule
    {
        public override void Load()
        {
        }

        private static readonly List<string> _typeList = new List<string>();
        private static object _syncRoot = new object();
        public void LoadType(Type faceType)
        {
            if (!_typeList.Contains(faceType.FullName.ToLower()))
            {
                lock (_syncRoot)
                {
                    if (!_typeList.Contains(faceType.FullName.ToLower()))
                    {
                        var dllName = faceType.Assembly.ManifestModule.Name;
                        var impDllName = dllName.Replace("IB", "B").Replace("IR", "R").Replace("IS", "S");
                        var basePath = AppDomain.CurrentDomain.BaseDirectory;
                        if (!basePath.Contains("bin"))
                        {
                            basePath += "bin\\";
                        }
                        var path = string.Format("{0}{1}", basePath, impDllName);
                        var assm = Assembly.LoadFile(path);
                        //AppDomain.CurrentDomain.Load(assm.FullName);
                        var types = assm.GetTypes();
                        foreach (var type in types)
                        {
                            var interfaces = type.GetInterfaces();
                            Type face = null;
                            if (interfaces.Count(f => f.Name.ToLower().EndsWith("bussiness")) > 0)
                            {
                                face = interfaces.FirstOrDefault(f => f.Name.ToLower().EndsWith("bussiness"));
                            }
                            else if (interfaces.Count(f => f.Name.ToLower().EndsWith("repository")) > 0)
                            {
                                face = interfaces.FirstOrDefault(f => f.Name.ToLower().EndsWith("repository"));
                            }
                            else if (interfaces.Count(f => f.Name.ToLower().EndsWith("service")) > 0)
                            {
                                face = interfaces.FirstOrDefault(f => f.Name.ToLower().EndsWith("service"));
                            }
                            if (face == null) continue;
                            var obj = TypeAccessor.CreateInstance(type);
                            var isBind = face.ContainsGenericParameters;
                            if (!isBind)
                                Bind(face).ToConstant(obj);
                            _typeList.Add(face.FullName.ToLower());
                        }
                    }
                }
            }
        }
    }
}
