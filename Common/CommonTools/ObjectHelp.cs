using System;
using System.Linq;
using System.Reflection;


namespace CommonTools
{


    public static class ObjectHelp
    {

        public static object GetDataByProperty(this object obj, string properTyName)
        {
            var propertys = obj.GetType().GetProperties();
            var property = propertys.FirstOrDefault(p => String.Equals(p.Name, properTyName, StringComparison.CurrentCultureIgnoreCase));
            return property == null ? null : property.GetValue(obj, null);
        }

        /// <summary>
        /// 根据对象名返回对象
        /// </summary>
        /// <param name="typeName">强类名</param>
        /// <param name="namespaceRef">命名空间</param>
        /// <returns>对象</returns>
        public static Type GetClassByName(string typeName, string namespaceRef)
        {
            var instance = Assembly.Load(namespaceRef).CreateInstance(typeName);
            return instance == null ? null : instance.GetType();
        }

        public static string DecimalToMoney(this decimal? data)
        {
            return string.Format("{0:#,##0.00}", data ?? 0);
        }

        public static string DecimalToMoney(this decimal data)
        {
            return string.Format("{0:#,##0.00}", data);
        }
        //#region 对象深度复制


        //struct Identity
        //{
        //    int _hashcode;
        //    RuntimeTypeHandle _type;


        //    public Identity(int hashcode, RuntimeTypeHandle type)
        //    {
        //        _hashcode = hashcode;
        //        _type = type;
        //    }
        //}
        ////缓存对象复制的方法。  
        //static readonly Dictionary<Type, Func<object, Dictionary<Identity, object>, object>> Methods1 = new Dictionary<Type, Func<object, Dictionary<Identity, object>, object>>();
        //static readonly Dictionary<Type, Action<object, Dictionary<Identity, object>, object>> Methods2 = new Dictionary<Type, Action<object, Dictionary<Identity, object>, object>>();


        //static List<FieldInfo> GetSettableFields(Type t)
        //{
        //    return t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();
        //}


        //static Func<object, Dictionary<Identity, object>, object> CreateCloneMethod1(Type type)
        //{
        //    var fields = GetSettableFields(type);
        //    var dm = new DynamicMethod(string.Format("Clone{0}", Guid.NewGuid()), typeof(object), new[] { typeof(object), typeof(Dictionary<Identity, object>) }, true);
        //    var il = dm.GetILGenerator();
        //    il.DeclareLocal(type);
        //    il.DeclareLocal(type);
        //    il.DeclareLocal(typeof(Identity));
        //    if (!type.IsArray)
        //    {
        //        il.Emit(OpCodes.Newobj, type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Stloc_1);
        //        il.Emit(OpCodes.Ldloca_S, 2);
        //        il.Emit(OpCodes.Ldarg_0);
        //        il.Emit(OpCodes.Castclass, type);
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Stloc_0);
        //        il.Emit(OpCodes.Callvirt, typeof(object).GetMethod("GetHashCode"));
        //        il.Emit(OpCodes.Ldtoken, type);
        //        il.Emit(OpCodes.Call, typeof(Identity).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int), typeof(RuntimeTypeHandle) }, null));
        //        il.Emit(OpCodes.Ldarg_1);
        //        il.Emit(OpCodes.Ldloc_2);
        //        il.Emit(OpCodes.Ldloc_1);
        //        il.Emit(OpCodes.Callvirt, typeof(Dictionary<Identity, object>).GetMethod("Add"));
        //        Type tmptype;
        //        foreach (var field in fields)
        //        {
        //            if (!field.FieldType.IsValueType && field.FieldType != typeof(String))
        //            {
        //                //不符合条件的字段，直接忽略，避免报错。  
        //                if ((field.FieldType.IsArray && (field.FieldType.GetArrayRank() > 1 || (!(tmptype = field.FieldType.GetElementType()).IsValueType && tmptype != typeof(String) && tmptype.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))) ||
        //                    (!field.FieldType.IsArray && field.FieldType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))
        //                    break;
        //                il.Emit(OpCodes.Ldloc_1);
        //                il.Emit(OpCodes.Ldloc_0);
        //                il.Emit(OpCodes.Ldfld, field);
        //                il.Emit(OpCodes.Ldarg_1);
        //                il.EmitCall(OpCodes.Call, typeof(ObjectHelp).GetMethod("CopyImpl", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(field.FieldType), null);
        //                il.Emit(OpCodes.Stfld, field);
        //            }
        //            else
        //            {
        //                il.Emit(OpCodes.Ldloc_1);
        //                il.Emit(OpCodes.Ldloc_0);
        //                il.Emit(OpCodes.Ldfld, field);
        //                il.Emit(OpCodes.Stfld, field);
        //            }
        //        }
        //        for (type = type.BaseType; type != null && type != typeof(object); type = type.BaseType)
        //        {
        //            //只需要查找基类的私有成员，共有或受保护的在派生类中直接被复制过了。  
        //            fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList();
        //            foreach (var field in fields)
        //            {
        //                if (!field.FieldType.IsValueType && field.FieldType != typeof(String))
        //                {
        //                    //不符合条件的字段，直接忽略，避免报错。  
        //                    if ((field.FieldType.IsArray && (field.FieldType.GetArrayRank() > 1 || (!(tmptype = field.FieldType.GetElementType()).IsValueType && tmptype != typeof(String) && tmptype.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))) ||
        //                        (!field.FieldType.IsArray && field.FieldType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))
        //                        break;
        //                    il.Emit(OpCodes.Ldloc_1);
        //                    il.Emit(OpCodes.Ldloc_0);
        //                    il.Emit(OpCodes.Ldfld, field);
        //                    il.Emit(OpCodes.Ldarg_1);
        //                    il.EmitCall(OpCodes.Call, typeof(ObjectHelp).GetMethod("CopyImpl", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(field.FieldType), null);
        //                    il.Emit(OpCodes.Stfld, field);
        //                }
        //                else
        //                {
        //                    il.Emit(OpCodes.Ldloc_1);
        //                    il.Emit(OpCodes.Ldloc_0);
        //                    il.Emit(OpCodes.Ldfld, field);
        //                    il.Emit(OpCodes.Stfld, field);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Type arraytype = type.GetElementType();
        //        var i = il.DeclareLocal(typeof(int));
        //        var lb1 = il.DefineLabel();
        //        var lb2 = il.DefineLabel();
        //        il.Emit(OpCodes.Ldarg_0);
        //        il.Emit(OpCodes.Castclass, type);
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Stloc_0);
        //        il.Emit(OpCodes.Ldlen);
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Ldc_I4_1);
        //        il.Emit(OpCodes.Sub);
        //        il.Emit(OpCodes.Stloc, i);
        //        il.Emit(OpCodes.Newarr, arraytype);
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Stloc_1);
        //        il.Emit(OpCodes.Ldloca_S, 2);
        //        il.Emit(OpCodes.Ldloc_0);
        //        il.Emit(OpCodes.Callvirt, typeof(object).GetMethod("GetHashCode"));
        //        il.Emit(OpCodes.Ldtoken, type);
        //        il.Emit(OpCodes.Call, typeof(Identity).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int), typeof(RuntimeTypeHandle) }, null));
        //        il.Emit(OpCodes.Ldarg_1);
        //        il.Emit(OpCodes.Ldloc_2);
        //        il.Emit(OpCodes.Ldloc_1);
        //        il.Emit(OpCodes.Callvirt, typeof(Dictionary<Identity, object>).GetMethod("Add"));
        //        il.Emit(OpCodes.Ldloc, i);
        //        il.Emit(OpCodes.Br, lb1);
        //        il.MarkLabel(lb2);
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Ldloc, i);
        //        il.Emit(OpCodes.Ldloc_0);
        //        il.Emit(OpCodes.Ldloc, i);
        //        il.Emit(OpCodes.Ldelem, arraytype);
        //        if (!arraytype.IsValueType && arraytype != typeof(String))
        //        {
        //            il.EmitCall(OpCodes.Call, typeof(ObjectHelp).GetMethod("CopyImpl", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(arraytype), null);
        //        }
        //        il.Emit(OpCodes.Stelem, arraytype);
        //        il.Emit(OpCodes.Ldloc, i);
        //        il.Emit(OpCodes.Ldc_I4_1);
        //        il.Emit(OpCodes.Sub);
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Stloc, i);
        //        il.MarkLabel(lb1);
        //        il.Emit(OpCodes.Ldc_I4_0);
        //        il.Emit(OpCodes.Clt);
        //        il.Emit(OpCodes.Brfalse, lb2);
        //    }
        //    il.Emit(OpCodes.Ret);


        //    return (Func<object, Dictionary<Identity, object>, object>)dm.CreateDelegate(typeof(Func<object, Dictionary<Identity, object>, object>));
        //}

        //static Action<object, Dictionary<Identity, object>, object> CreateCloneMethod2(Type type)
        //{
        //    var fields = GetSettableFields(type);
        //    var dm = new DynamicMethod(string.Format("Copy{0}", Guid.NewGuid()), null, new[] { typeof(object), typeof(Dictionary<Identity, object>), typeof(object) }, true);
        //    var il = dm.GetILGenerator();
        //    il.DeclareLocal(type);
        //    il.DeclareLocal(type);
        //    il.DeclareLocal(typeof(Identity));
        //    if (!type.IsArray)
        //    {
        //        il.Emit(OpCodes.Ldarg_2);
        //        il.Emit(OpCodes.Castclass, type);
        //        il.Emit(OpCodes.Stloc_1);
        //        il.Emit(OpCodes.Ldarg_0);
        //        il.Emit(OpCodes.Castclass, type);
        //        il.Emit(OpCodes.Stloc_0);
        //        Type tmptype;
        //        foreach (var field in fields)
        //        {
        //            if (!field.FieldType.IsValueType && field.FieldType != typeof(String))
        //            {
        //                //不符合条件的字段，直接忽略，避免报错。  
        //                if ((field.FieldType.IsArray && (field.FieldType.GetArrayRank() > 1 || (!(tmptype = field.FieldType.GetElementType()).IsValueType && tmptype != typeof(String) && tmptype.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))) ||
        //                    (!field.FieldType.IsArray && field.FieldType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))
        //                    break;
        //                il.Emit(OpCodes.Ldloc_1);
        //                il.Emit(OpCodes.Ldloc_0);
        //                il.Emit(OpCodes.Ldfld, field);
        //                il.Emit(OpCodes.Ldarg_1);
        //                il.EmitCall(OpCodes.Call, typeof(ObjectHelp).GetMethod("CopyImpl", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(field.FieldType), null);
        //                il.Emit(OpCodes.Stfld, field);
        //            }
        //            else
        //            {
        //                il.Emit(OpCodes.Ldloc_1);
        //                il.Emit(OpCodes.Ldloc_0);
        //                il.Emit(OpCodes.Ldfld, field);
        //                il.Emit(OpCodes.Stfld, field);
        //            }
        //        }
        //        for (type = type.BaseType; type != null && type != typeof(object); type = type.BaseType)
        //        {
        //            //只需要查找基类的私有成员，共有或受保护的在派生类中直接被复制过了。  
        //            fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList();
        //            foreach (var field in fields)
        //            {
        //                if (!field.FieldType.IsValueType && field.FieldType != typeof(String))
        //                {
        //                    //不符合条件的字段，直接忽略，避免报错。  
        //                    if ((field.FieldType.IsArray && (field.FieldType.GetArrayRank() > 1 || (!(tmptype = field.FieldType.GetElementType()).IsValueType && tmptype != typeof(String) && tmptype.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))) ||
        //                        (!field.FieldType.IsArray && field.FieldType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))
        //                        break;
        //                    il.Emit(OpCodes.Ldloc_1);
        //                    il.Emit(OpCodes.Ldloc_0);
        //                    il.Emit(OpCodes.Ldfld, field);
        //                    il.Emit(OpCodes.Ldarg_1);
        //                    il.EmitCall(OpCodes.Call, typeof(ObjectHelp).GetMethod("CopyImpl", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(field.FieldType), null);
        //                    il.Emit(OpCodes.Stfld, field);
        //                }
        //                else
        //                {
        //                    il.Emit(OpCodes.Ldloc_1);
        //                    il.Emit(OpCodes.Ldloc_0);
        //                    il.Emit(OpCodes.Ldfld, field);
        //                    il.Emit(OpCodes.Stfld, field);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Type arraytype = type.GetElementType();
        //        var i = il.DeclareLocal(typeof(int));
        //        var lb1 = il.DefineLabel();
        //        var lb2 = il.DefineLabel();
        //        il.Emit(OpCodes.Ldarg_0);
        //        il.Emit(OpCodes.Castclass, type);
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Stloc_0);
        //        il.Emit(OpCodes.Ldlen);
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Ldc_I4_1);
        //        il.Emit(OpCodes.Sub);
        //        il.Emit(OpCodes.Stloc, i);
        //        il.Emit(OpCodes.Ldarg_2);
        //        il.Emit(OpCodes.Castclass, type);
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Stloc_1);
        //        il.Emit(OpCodes.Ldloc, i);
        //        il.Emit(OpCodes.Br, lb1);
        //        il.MarkLabel(lb2);
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Ldloc, i);
        //        il.Emit(OpCodes.Ldloc_0);
        //        il.Emit(OpCodes.Ldloc, i);
        //        il.Emit(OpCodes.Ldelem, arraytype);
        //        if (!arraytype.IsValueType && arraytype != typeof(String))
        //        {
        //            il.EmitCall(OpCodes.Call, typeof(ObjectHelp).GetMethod("CopyImpl", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(arraytype), null);
        //        }
        //        il.Emit(OpCodes.Stelem, arraytype);
        //        il.Emit(OpCodes.Ldloc, i);
        //        il.Emit(OpCodes.Ldc_I4_1);
        //        il.Emit(OpCodes.Sub);
        //        il.Emit(OpCodes.Dup);
        //        il.Emit(OpCodes.Stloc, i);
        //        il.MarkLabel(lb1);
        //        il.Emit(OpCodes.Ldc_I4_0);
        //        il.Emit(OpCodes.Clt);
        //        il.Emit(OpCodes.Brfalse, lb2);
        //    }
        //    il.Emit(OpCodes.Ret);


        //    return (Action<object, Dictionary<Identity, object>, object>)dm.CreateDelegate(typeof(Action<object, Dictionary<Identity, object>, object>));
        //}


        ///// <summary>  
        ///// 创建对象深度复制的副本  
        ///// </summary>  
        //public static T ToObjectCopy<T>(this T source) where T : class
        //{
        //    var type = source.GetType();
        //    var objects = new Dictionary<Identity, object>();//存放内嵌引用类型的复制链，避免构成一个环。  
        //    Func<object, Dictionary<Identity, object>, object> method;
        //    if (!Methods1.TryGetValue(type, out method))
        //    {
        //        method = CreateCloneMethod1(type);
        //        Methods1.Add(type, method);
        //    }
        //    return (T)method(source, objects);
        //}




        ///// <summary>  
        ///// 将source对象的所有属性复制到target对象中，深度复制  
        ///// </summary>  
        //public static void ObjectHelpTo<T>(this T source, T target) where T : class
        //{
        //    if (target == null)
        //        throw new Exception("将要复制的目标未初始化");
        //    var type = source.GetType();
        //    if (type != target.GetType())
        //        throw new Exception("要复制的对象类型不同，无法复制");
        //    var objects = new Dictionary<Identity, object>
        //    {
        //        {new Identity(source.GetHashCode(), type.TypeHandle), source}
        //    };//存放内嵌引用类型的复制链，避免构成一个环。  
        //    Action<object, Dictionary<Identity, object>, object> method;
        //    if (!Methods2.TryGetValue(type, out method))
        //    {
        //        method = CreateCloneMethod2(type);
        //        Methods2.Add(type, method);
        //    }
        //    method(source, objects, target);
        //}
        //#endregion
    }
}


