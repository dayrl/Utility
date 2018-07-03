/*************************************************************************
// Copyright (C) 2017  Qinchuan IoT Technology Co Ltd. 
// All Rights Reserved 
//
// FileName: D:\workspace\GMIS7\QCWebAPI\Zdd.Utility\Graphs\GridGraphBase.cs
// Function Description：
//			GridGraphBase.cs
// 
// Creator:	zdd (Administrator)
// Create Time:	6:11:2017   15:17
//	
// Change History:
// Editor:
// Time:
// Comment:
//
// Version: V1.0.0
/*************************************************************************/
using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public abstract class GridGraphBase : GraphBase
    {
        private int minScaleValue = 0;
        private int maxScaleValue;
        private bool honorScale = false;
        private bool showGrid = true;
        private int gridSpacingValue = 0;
        private int marginForTextOnAxis = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridGraphBase"/> class.
        /// </summary>
        public GridGraphBase()
        {
            MarginForTextOnAxis = 20;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridGraphBase"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public GridGraphBase(Size size)
            : base(size)
        {
            MarginForTextOnAxis = 20;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridGraphBase"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public GridGraphBase(int width, int height)
            : base(width, height)
        {
            MarginForTextOnAxis = 20;
        }

        /// <summary>
        /// 获取或设置MinScaleValue
        /// </summary>
        /// <value></value>
        public int MinScaleValue
        {
            get { return minScaleValue; }
            set
            {
                minScaleValue = value;
                HonorScale = true;
            }
        }

        /// <summary>
        /// 获取或设置MaxScaleValue
        /// </summary>
        /// <value></value>
        public int MaxScaleValue
        {
            get { return maxScaleValue; }
            set
            {
                maxScaleValue = value;
                HonorScale = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [honor scale].
        /// </summary>
        /// <value><c>true</c> if [honor scale]; otherwise, <c>false</c>.</value>
        public bool HonorScale
        {
            get { return honorScale; }
            set
            {
                honorScale = value;
                if (!honorScale)
                    minScaleValue = 0;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show grid].
        /// </summary>
        /// <value><c>true</c> if [show grid]; otherwise, <c>false</c>.</value>
        public bool ShowGrid
        {
            get { return showGrid; }
            set { showGrid = value; }
        }

        /// <summary>
        /// 获取或设置GridSpacingValue
        /// </summary>
        /// <value></value>
        public int GridSpacingValue
        {
            get { return gridSpacingValue; }
            set { gridSpacingValue = value; }
        }

        /// <summary>
        /// 获取或设置MarginForTextOnAxis
        /// </summary>
        /// <value></value>
        public int MarginForTextOnAxis
        {
            get { return marginForTextOnAxis; }
            set { marginForTextOnAxis = value; }
        }
    }
}