using RevStack.Client.Http.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Types
{
    public class RSDocument : Dictionary<string, object>
    {
        #region Properties which holds orient specific fields

        public RSID RID
        {
            get
            {
                return this.HasField("@RID") ? this.GetField<RSID>("@RID") : null;
            }
            set { this.SetField("@RID", value); }
        }

        public int RSVersion
        {
            get
            {
                return this.GetField<int>("@RSVersion");
            }
            set { this.SetField("@RSVersion", value); }
        }

        public RSRecordType RSType
        {
            get
            {
                return this.GetField<RSRecordType>("@RSType");
            }
            set { this.SetField("@RSType", value); }
        }

        public short RSClassId
        {
            get
            {
                return this.GetField<short>("@RSClassId");
            }
            set { this.SetField("@RSClassId", value); }
        }

        public string RSClassName
        {
            get
            {
                return this.GetField<string>("@RSClassName");
            }
            set { this.SetField("@RSClassName", value); }
        }

        #endregion

        public T GetField<T>(string fieldPath)
        {
            Type type = typeof(T);
            T value;

            if (type.IsPrimitive || type.IsArray || (type.Name == "String"))
            {
                value = default(T);
            }
            else
            {
                value = (T)Activator.CreateInstance(type);
            }

            if (fieldPath.Contains("."))
            {
                var fields = fieldPath.Split('.');
                int iteration = 1;
                RSDocument embeddedDocument = this;

                foreach (var field in fields)
                {
                    if (iteration == fields.Length)
                    {
                        // if value is collection type, get element type and enumerate over its elements
                        if (value is IList)
                        {
                            Type elementType = ((IEnumerable)value).GetType().GetGenericArguments()[0];
                            IEnumerator enumerator = ((IEnumerable)embeddedDocument[field]).GetEnumerator();

                            while (enumerator.MoveNext())
                            {
                                // if current element is ODocument type which is dictionary<string, object>
                                // map its dictionary data to element instance
                                if (enumerator.Current is RSDocument)
                                {
                                    var instance = Activator.CreateInstance(elementType);
                                    ((RSDocument)enumerator.Current).Map(ref instance);

                                    ((IList)value).Add(instance);
                                }
                                else
                                {
                                    ((IList)value).Add(enumerator.Current);
                                }
                            }
                        }
                        else if (type.Name == "HashSet`1")
                        {
                            Type elementType = ((IEnumerable)value).GetType().GetGenericArguments()[0];
                            IEnumerator enumerator = ((IEnumerable)this[fieldPath]).GetEnumerator();

                            var addMethod = type.GetMethod("Add");

                            while (enumerator.MoveNext())
                            {
                                // if current element is RSDocument type which is Dictionary<string, object>
                                // map its dictionary data to element instance
                                if (enumerator.Current is RSDocument)
                                {
                                    var instance = Activator.CreateInstance(elementType);
                                    ((RSDocument)enumerator.Current).Map(ref instance);

                                    addMethod.Invoke(value, new object[] { instance });
                                }
                                else
                                {
                                    addMethod.Invoke(value, new object[] { enumerator.Current });
                                }
                            }
                        }
                        else
                        {
                            value = (T)embeddedDocument[field];
                        }
                        break;
                    }

                    if (embeddedDocument.ContainsKey(field))
                    {
                        embeddedDocument = (RSDocument)embeddedDocument[field];
                        iteration++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                if (this.ContainsKey(fieldPath))
                {
                    // if value is list or set type, get element type and enumerate over its elements
                    if (value is IList)
                    {
                        Type elementType = ((IEnumerable)value).GetType().GetGenericArguments()[0];
                        IEnumerator enumerator = ((IEnumerable)this[fieldPath]).GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            // if current element is RSDocument type which is Dictionary<string, object>
                            // map its dictionary data to element instance
                            if (enumerator.Current is RSDocument)
                            {
                                var instance = Activator.CreateInstance(elementType);
                                ((RSDocument)enumerator.Current).Map(ref instance);

                                ((IList)value).Add(instance);
                            }
                            else
                            {
                                ((IList)value).Add(enumerator.Current);
                            }
                        }
                    }
                    else if (type.Name == "HashSet`1")
                    {
                        Type elementType = ((IEnumerable)value).GetType().GetGenericArguments()[0];
                        IEnumerator enumerator = ((IEnumerable)this[fieldPath]).GetEnumerator();

                        var addMethod = type.GetMethod("Add");

                        while (enumerator.MoveNext())
                        {
                            // if current element is ODocument type which is Dictionary<string, object>
                            // map its dictionary data to element instance
                            if (enumerator.Current is RSDocument)
                            {
                                var instance = Activator.CreateInstance(elementType);
                                ((RSDocument)enumerator.Current).Map(ref instance);

                                addMethod.Invoke(value, new object[] { instance });
                            }
                            else
                            {
                                addMethod.Invoke(value, new object[] { enumerator.Current });
                            }
                        }
                    }
                    else
                    {
                        value = (T)this[fieldPath];
                    }
                }
            }

            return value;
        }

        public RSDocument SetField<T>(string fieldPath, T value)
        {
            if (fieldPath.Contains("."))
            {
                var fields = fieldPath.Split('.');
                int iteration = 1;
                RSDocument embeddedDocument = this;

                foreach (var field in fields)
                {
                    if (iteration == fields.Length)
                    {
                        if (embeddedDocument.ContainsKey(field))
                        {
                            embeddedDocument[field] = value;
                        }
                        else
                        {
                            embeddedDocument.Add(field, value);
                        }
                        break;
                    }

                    if (embeddedDocument.ContainsKey(field))
                    {
                        embeddedDocument = (RSDocument)embeddedDocument[field];
                    }
                    else
                    {
                        // if document which contains the field doesn't exist create it first
                        RSDocument tempDocument = new RSDocument();
                        embeddedDocument.Add(field, tempDocument);
                        embeddedDocument = tempDocument;
                    }

                    iteration++;
                }
            }
            else
            {
                if (this.ContainsKey(fieldPath))
                {
                    this[fieldPath] = value;
                }
                else
                {
                    this.Add(fieldPath, value);
                }
            }

            return this;
        }

        public bool HasField(string fieldPath)
        {
            bool contains = false;

            if (fieldPath.Contains("."))
            {
                var fields = fieldPath.Split('.');
                int iteration = 1;
                RSDocument embeddedDocument = this;

                foreach (var field in fields)
                {
                    if (iteration == fields.Length)
                    {
                        contains = embeddedDocument.ContainsKey(field);
                        break;
                    }

                    if (embeddedDocument.ContainsKey(field))
                    {
                        embeddedDocument = (RSDocument)embeddedDocument[field];
                        iteration++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                contains = this.ContainsKey(fieldPath);
            }

            return contains;
        }

        public T To<T>() where T : class, new()
        {
            T genericObject = new T();

            genericObject = (T)ToObject<T>(genericObject, "");

            return genericObject;
        }

        public string Serialize()
        {
            return RecordSerializer.Serialize(this);
        }

        public static RSDocument Deserialize(string recordString)
        {
            return RecordSerializer.Deserialize(recordString);
        }

        public static RSDocument ToDocument<T>(T genericObject)
        {
            RSDocument document = new RSDocument();
            Type genericObjectType = genericObject.GetType();

            // TODO: recursive mapping of nested/embedded objects
            foreach (PropertyInfo propertyInfo in genericObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                string propertyName = propertyInfo.Name;

                // serialize following properties into dedicated fields in ODocument
                if (propertyName.Equals("RSID"))
                {
                    document.RID = (RSID)propertyInfo.GetValue(genericObject, null);
                    continue;
                }
                else if (propertyName.Equals("RSVersion"))
                {
                    document.RSVersion = (int)propertyInfo.GetValue(genericObject, null);
                    continue;
                }
                else if (propertyName.Equals("RSClassId"))
                {
                    document.RSClassId = (short)propertyInfo.GetValue(genericObject, null);
                    continue;
                }
                else if (propertyName.Equals("RSClassName"))
                {
                    document.RSClassName = (string)propertyInfo.GetValue(genericObject, null);
                    continue;
                }

                bool isSerializable = true;
                object[] oProperties = propertyInfo.GetCustomAttributes(typeof(RSProperty), true);

                if (oProperties.Any())
                {
                    RSProperty RSProperty = oProperties.First() as RSProperty;
                    if (RSProperty != null)
                    {
                        propertyName = RSProperty.Alias;
                        isSerializable = RSProperty.Serializable;
                    }
                }

                if (isSerializable)
                {
                    document.SetField(propertyName, propertyInfo.GetValue(genericObject, null));
                }
            }

            return document;
        }

        private T ToObject<T>(T genericObject, string path) where T : class, new()
        {
            Type genericObjectType = genericObject.GetType();

            if (genericObjectType.Name.Equals("RSDocument"))
            {
                foreach (KeyValuePair<string, object> item in this)
                {
                    (genericObject as RSDocument).SetField(item.Key, item.Value);
                }
            }
            else
            {
                foreach (PropertyInfo propertyInfo in genericObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    string propertyName = propertyInfo.Name;

                    // serialize orient specific fields into dedicated properties
                    if (propertyName.Equals("RID"))
                    {
                        propertyInfo.SetValue(genericObject, this.RID, null);
                        continue;
                    }
                    else if (propertyName.Equals("RSVersion"))
                    {
                        propertyInfo.SetValue(genericObject, this.RSVersion, null);
                        continue;
                    }
                    else if (propertyName.Equals("RSClassId"))
                    {
                        propertyInfo.SetValue(genericObject, this.RSClassId, null);
                        continue;
                    }
                    else if (propertyName.Equals("RSClassName"))
                    {
                        propertyInfo.SetValue(genericObject, this.RSClassName, null);
                        continue;
                    }

                    object[] oProperties = propertyInfo.GetCustomAttributes(typeof(RSProperty), true);

                    if (oProperties.Any())
                    {
                        RSProperty RSProperty = oProperties.First() as RSProperty;
                        if (RSProperty != null)
                        {
                            propertyName = RSProperty.Alias;
                        }
                    }

                    string fieldPath = path + (path != "" ? "." : "") + propertyName;

                    if ((propertyInfo.PropertyType.IsArray || propertyInfo.PropertyType.IsGenericType))
                    {
                        if (this.HasField(fieldPath))
                        {
                            object propertyValue = this.GetField<object>(fieldPath);

                            IList collection = (IList)propertyValue;

                            if (collection.Count > 0)
                            {
                                // create instance of property type
                                object collectionInstance = Activator.CreateInstance(propertyInfo.PropertyType, collection.Count);

                                for (int i = 0; i < collection.Count; i++)
                                {
                                    // collection is simple array
                                    if (propertyInfo.PropertyType.IsArray)
                                    {
                                        ((object[])collectionInstance)[i] = collection[i];
                                    }
                                    // collection is generic
                                    else if (propertyInfo.PropertyType.IsGenericType && (propertyValue is IEnumerable))
                                    {
                                        Type elementType = collection[i].GetType();

                                        // generic collection consists of basic types or ORIDs
                                        if (elementType.IsPrimitive ||
                                            (elementType == typeof(string)) ||
                                            (elementType == typeof(DateTime)) ||
                                            (elementType == typeof(decimal)) ||
                                            (elementType == typeof(RSID)))
                                        {
                                            ((IList)collectionInstance).Add(collection[i]);
                                        }
                                        // generic collection consists of generic type which should be parsed
                                        else
                                        {
                                            // create instance object based on first element of generic collection
                                            object instance = Activator.CreateInstance(propertyInfo.PropertyType.GetGenericArguments().First(), null);

                                            ((IList)collectionInstance).Add(ToObject(instance, fieldPath));
                                        }
                                    }
                                    else
                                    {
                                        object v = Activator.CreateInstance(collection[i].GetType(), collection[i]);

                                        ((IList)collectionInstance).Add(v);
                                    }
                                }

                                propertyInfo.SetValue(genericObject, collectionInstance, null);
                            }
                        }
                    }
                    // property is class except the string or ORID type since string and ORID values are parsed differently
                    else if (propertyInfo.PropertyType.IsClass &&
                        (propertyInfo.PropertyType.Name != "String") &&
                        (propertyInfo.PropertyType.Name != "RID"))
                    {
                        // create object instance of embedded class
                        object instance = Activator.CreateInstance(propertyInfo.PropertyType);

                        object o = ToObject(instance, fieldPath);
                        propertyInfo.SetValue(genericObject, o, null);
                    }
                    // property is basic type
                    else
                    {
                        if (this.HasField(fieldPath))
                        {
                            object propertyValue = this.GetField<object>(fieldPath);

                            propertyInfo.SetValue(genericObject, propertyValue, null);
                        }
                    }
                }
            }

            return genericObject;
        }

        // maps ODocument fields to specified object
        private void Map(ref object obj)
        {
            if (obj is Dictionary<string, object>)
            {
                obj = this;
            }
            else
            {
                Type objType = obj.GetType();

                foreach (KeyValuePair<string, object> item in this)
                {
                    PropertyInfo property = objType.GetProperty(item.Key);

                    if (property != null)
                    {
                        property.SetValue(obj, item.Value, null);
                    }
                }
            }
        }
    }
}
