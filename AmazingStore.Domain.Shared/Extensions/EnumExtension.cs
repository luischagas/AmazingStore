﻿using System;
using System.Linq;
using System.Reflection;

namespace AmazingStore.Domain.Shared.Extensions
{
    public static class EnumExtension
    {

        #region Public Methods

        public static string GetDescription(this Enum genericEnum)
        {
            Type genericEnumType = genericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(genericEnum.ToString());

            if (memberInfo.Length > 0)
            {
                var attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

                if (attribs.Any())
                    return ((System.ComponentModel.DescriptionAttribute)attribs.ElementAt(0)).Description;

                return genericEnum.ToString();
            }

            return "n/a";
        }

        #endregion Public Methods

    }
}