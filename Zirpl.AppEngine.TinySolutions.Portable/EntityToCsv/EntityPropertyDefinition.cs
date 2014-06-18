﻿using System;

namespace Zirpl.AppEngine.TinySolutions.EntityToCsv
{
    public class EntityPropertyDefinition<TObject>
    {

        public String HeaderText { get; set; }
        public Func<TObject, Object> ValueFunction { get; set; }
        public PropertyType PropertyType { get; set; }
    }
}
