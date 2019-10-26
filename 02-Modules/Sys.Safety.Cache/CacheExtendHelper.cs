using Basic.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache
{
    public static class CacheExtendHelper
    {
        public static void CopyProperties<T>(this T instance, Dictionary<string, object> updateItems)
        {
            if (instance == null)
                return;

            Type type = instance.GetType();            
            string typeName = type.Name;
            foreach (var item in updateItems)
            {
                //获取属性
                //PropertyInfo itemProperty = instance.GetType().GetProperty(item.Key);
                PropertyInfo itemProperty =type.GetProperty(item.Key);

                //如果属性存在,则给iteminfo对应属性赋值
                if (itemProperty != null)
                {
                    //如果value是复杂属性
                    if (item.Value is Dictionary<string, object>)
                    {
                        var childItems = item.Value as Dictionary<string, object>;
                        var pivalue = itemProperty.GetValue(instance);

                        if (pivalue == null)
                        {
                            //如果pivalue为空,则可以反射创建一个实例。但不确定反射程序集,所以在此不创建实例,由缓存加载时初始化
                        }

                        //复杂属性递归赋值
                        pivalue.CopyProperties(childItems);
                        try
                        {
                            itemProperty.SetValue(instance, Convert.ChangeType(pivalue, itemProperty.PropertyType));
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("更新缓存信息出错：" + typeName + " " + item.Key + "\r\n" + ex.Message);
                        }
                    }
                    else
                    {
                        Type baseType = Nullable.GetUnderlyingType(itemProperty.PropertyType);

                        try
                        {
                            //如果是可空类型,则判断value是否为空并分别赋值
                            if (baseType != null)
                            {
                                if (item.Value == null)
                                    itemProperty.SetValue(instance, Convert.ChangeType(item.Value, typeof(Nullable)));
                                else
                                    itemProperty.SetValue(instance, Convert.ChangeType(item.Value, baseType));
                            }
                            else
                            {
                                itemProperty.SetValue(instance, Convert.ChangeType(item.Value, itemProperty.PropertyType));
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("更新缓存信息出错：" + typeName + " " + item.Key + "\r\n" + ex.Message);
                        }
                    }
                }
            }
        }
    }
}
