﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Facer.FaceApi
{
    public struct IdentificationInfo
    {
        public IdentificationInfo(FaceRectangle r, float c)
        {
            Rectangle = r;
            Confidence = c;
        }
        public FaceRectangle Rectangle { get; set; }
        public float Confidence { get; set; }
    }
}
