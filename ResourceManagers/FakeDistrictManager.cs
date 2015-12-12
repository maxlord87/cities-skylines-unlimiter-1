﻿using ColossalFramework;
using ColossalFramework.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;
using EightyOne.Attributes;

//TODO(earalov): review this class
namespace EightyOne.ResourceManagers
{
    [TargetType(typeof(DistrictManager))]
    public class FakeDistrictManager : DistrictManager
    {
        public new class Data : IDataContainer
        {
            public void AfterDeserialize(DataSerializer s)
            {
                Singleton<LoadingManager>.instance.WaitUntilEssentialScenesLoaded();
                DistrictManager instance = Singleton<DistrictManager>.instance;
                District[] buffer = instance.m_districts.m_buffer;
                DistrictManager.Cell[] districtGrid = FakeDistrictManager.districtGrid;
                int num = districtGrid.Length;
                for (int i = 0; i < num; i++)
                {
                    DistrictManager.Cell cell = districtGrid[i];
                    District[] expr_60_cp_0 = buffer;
                    byte expr_60_cp_1 = cell.m_district1;
                    expr_60_cp_0[(int)expr_60_cp_1].m_totalAlpha = expr_60_cp_0[(int)expr_60_cp_1].m_totalAlpha + (uint)cell.m_alpha1;
                    District[] expr_80_cp_0 = buffer;
                    byte expr_80_cp_1 = cell.m_district2;
                    expr_80_cp_0[(int)expr_80_cp_1].m_totalAlpha = expr_80_cp_0[(int)expr_80_cp_1].m_totalAlpha + (uint)cell.m_alpha2;
                    District[] expr_A0_cp_0 = buffer;
                    byte expr_A0_cp_1 = cell.m_district3;
                    expr_A0_cp_0[(int)expr_A0_cp_1].m_totalAlpha = expr_A0_cp_0[(int)expr_A0_cp_1].m_totalAlpha + (uint)cell.m_alpha3;
                    District[] expr_C0_cp_0 = buffer;
                    byte expr_C0_cp_1 = cell.m_district4;
                    expr_C0_cp_0[(int)expr_C0_cp_1].m_totalAlpha = expr_C0_cp_0[(int)expr_C0_cp_1].m_totalAlpha + (uint)cell.m_alpha4;
                }
                instance.m_districtCount = (int)(instance.m_districts.ItemCount() - 1u);
                instance.AreaModified(0, 0, 511, 511, true);
                instance.NamesModified();
            }

            public void Deserialize(DataSerializer s)
            {
                var districtGrid = new DistrictManager.Cell[GRID * GRID];
                EncodedArray.Byte @byte = EncodedArray.Byte.BeginRead(s);
                int num2 = districtGrid.Length;
                for (int num21 = 0; num21 < num2; num21++)
                {
                    districtGrid[num21].m_district1 = @byte.Read();
                }
                for (int num22 = 0; num22 < num2; num22++)
                {
                    districtGrid[num22].m_district2 = @byte.Read();
                }
                for (int num23 = 0; num23 < num2; num23++)
                {
                    districtGrid[num23].m_district3 = @byte.Read();
                }
                for (int num24 = 0; num24 < num2; num24++)
                {
                    districtGrid[num24].m_district4 = @byte.Read();
                }
                for (int num25 = 0; num25 < num2; num25++)
                {
                    districtGrid[num25].m_alpha1 = @byte.Read();
                }
                for (int num26 = 0; num26 < num2; num26++)
                {
                    districtGrid[num26].m_alpha2 = @byte.Read();
                }
                for (int num27 = 0; num27 < num2; num27++)
                {
                    districtGrid[num27].m_alpha3 = @byte.Read();
                }
                for (int num28 = 0; num28 < num2; num28++)
                {
                    districtGrid[num28].m_alpha4 = @byte.Read();
                }
                @byte.EndRead();

                FakeDistrictManager.districtGrid = districtGrid;
            }

            public void Serialize(DataSerializer s)
            {
                var districtGrid = FakeDistrictManager.districtGrid;
                int num2 = districtGrid.Length;
                EncodedArray.Byte @byte = EncodedArray.Byte.BeginWrite(s);
                for (int num19 = 0; num19 < num2; num19++)
                {
                    @byte.Write(districtGrid[num19].m_district1);
                }
                for (int num20 = 0; num20 < num2; num20++)
                {
                    @byte.Write(districtGrid[num20].m_district2);
                }
                for (int num21 = 0; num21 < num2; num21++)
                {
                    @byte.Write(districtGrid[num21].m_district3);
                }
                for (int num22 = 0; num22 < num2; num22++)
                {
                    @byte.Write(districtGrid[num22].m_district4);
                }
                for (int num23 = 0; num23 < num2; num23++)
                {
                    @byte.Write(districtGrid[num23].m_alpha1);
                }
                for (int num24 = 0; num24 < num2; num24++)
                {
                    @byte.Write(districtGrid[num24].m_alpha2);
                }
                for (int num25 = 0; num25 < num2; num25++)
                {
                    @byte.Write(districtGrid[num25].m_alpha3);
                }
                for (int num26 = 0; num26 < num2; num26++)
                {
                    @byte.Write(districtGrid[num26].m_alpha4);
                }
                @byte.EndWrite();
            }
        }

        public static int GRID = 900;
        public static int HALFGRID = 450;
        public static DistrictManager.Cell[] districtGrid;
        private static Color32[] colorBuffer;
        private static int[] distanceBuffer;
        private static int[] indexBuffer;

        private static Texture2D districtTexture1;
        private static Texture2D districtTexture2;

        private static FieldInfo modifiedX1Field;
        private static FieldInfo modifiedZ1Field;
        private static FieldInfo modifiedX2Field;
        private static FieldInfo modifiedZ2Field;
        private static FieldInfo fullUpdateField;
        private static FieldInfo modifyLockField;
        private static FieldInfo namesModifiedField;
        private static FieldInfo areaMaterialField;

        private static int ID_Districts1;
        private static int ID_Districts2;
        private static int ID_DistrictMapping;
        private static int ID_Highlight1;
        private static int ID_Highlight2;

        private static TempDistrictData[] m_tempData;

        private static MethodInfo ReleaseDistrictImplementationMethod = typeof(DistrictManager).GetMethod("ReleaseDistrictImplementation",
            BindingFlags.Instance | BindingFlags.NonPublic);

        private struct TempDistrictData
        {
            public int m_averageX;
            public int m_averageZ;
            public int m_bestScore;
            public int m_divider;
            public int m_bestLocation;
        }

        public static void OnDestroy()
        {
            if (districtTexture1 != null)
            {
                UnityEngine.Object.Destroy(districtTexture1);
                districtTexture1 = null;
            }
            if (districtTexture2 != null)
            {
                UnityEngine.Object.Destroy(districtTexture2);
                districtTexture2 = null;
            }
            districtGrid = null;
        }

        public static void Init()
        {
            if (districtGrid == null)
            {
                districtGrid = new DistrictManager.Cell[GRID * GRID];
                for (int i = 0; i < districtGrid.Length; i++)
                {
                    districtGrid[i].m_district1 = 0;
                    districtGrid[i].m_district2 = 1;
                    districtGrid[i].m_district3 = 2;
                    districtGrid[i].m_district4 = 3;
                    districtGrid[i].m_alpha1 = 255;
                    districtGrid[i].m_alpha2 = 0;
                    districtGrid[i].m_alpha3 = 0;
                    districtGrid[i].m_alpha4 = 0;
                }

                var oldGrid = DistrictManager.instance.m_districtGrid;
                int diff = (GRID - DISTRICTGRID_RESOLUTION) / 2;
                for (var i = 0; i < DISTRICTGRID_RESOLUTION; i += 1)
                {
                    for (var j = 0; j < DISTRICTGRID_RESOLUTION; j += 1)
                    {
                        districtGrid[(j + diff) * GRID + (i + diff)] = oldGrid[j * DISTRICTGRID_RESOLUTION + i];
                    }
                }
            }
            colorBuffer = new Color32[GRID * GRID];
            distanceBuffer = new int[HALFGRID * HALFGRID];
            indexBuffer = new int[HALFGRID * HALFGRID];
            m_tempData = new TempDistrictData[128];

            modifiedX1Field = typeof(DistrictManager).GetField("m_modifiedX1", BindingFlags.Instance | BindingFlags.NonPublic);
            modifiedZ1Field = typeof(DistrictManager).GetField("m_modifiedZ1", BindingFlags.Instance | BindingFlags.NonPublic);
            modifiedX2Field = typeof(DistrictManager).GetField("m_modifiedX2", BindingFlags.Instance | BindingFlags.NonPublic);
            modifiedZ2Field = typeof(DistrictManager).GetField("m_modifiedZ2", BindingFlags.Instance | BindingFlags.NonPublic);
            fullUpdateField = typeof(DistrictManager).GetField("m_fullUpdate", BindingFlags.Instance | BindingFlags.NonPublic);
            modifyLockField = typeof(DistrictManager).GetField("m_modifyLock", BindingFlags.Instance | BindingFlags.NonPublic);
            namesModifiedField = typeof(DistrictManager).GetField("m_namesModified", BindingFlags.Instance | BindingFlags.NonPublic);
            areaMaterialField = typeof(DistrictManager).GetField("m_areaMaterial", BindingFlags.Instance | BindingFlags.NonPublic);

            districtTexture1 = new Texture2D(GRID, GRID, TextureFormat.ARGB32, false, true);
            districtTexture2 = new Texture2D(GRID, GRID, TextureFormat.ARGB32, false, true);
            districtTexture1.wrapMode = TextureWrapMode.Clamp;
            districtTexture2.wrapMode = TextureWrapMode.Clamp;
            ID_Districts1 = Shader.PropertyToID("_Districts1");
            ID_Districts2 = Shader.PropertyToID("_Districts2");
            ID_DistrictMapping = Shader.PropertyToID("_DistrictMapping");
            ID_Highlight1 = Shader.PropertyToID("_Highlight1");
            ID_Highlight2 = Shader.PropertyToID("_Highlight2");
        }

        [ReplaceMethod]
        protected override void BeginOverlayImpl(RenderManager.CameraInfo cameraInfo)
        {
            var areaMaterial = (Material)areaMaterialField.GetValue(this);
            if ((!this.DistrictsVisible && !this.DistrictsInfoVisible) || areaMaterial == null)
            {
                return;
            }
            areaMaterial.SetTexture(ID_Districts1, districtTexture1);
            areaMaterial.SetTexture(ID_Districts2, districtTexture2);
            Vector4 vector;
            //begin mod
            vector.z = 1 / (19.2f * GRID);
            //end mod
            vector.x = 0.5f;
            vector.y = 0.5f;
            vector.w = ((this.HighlightDistrict <= 0) ? 0f : 1f);
            areaMaterial.SetVector(ID_DistrictMapping, vector);
            Color32 c = new Color32(128, 128, 128, 128);
            Color32 c2 = new Color32(128, 128, 128, 128);
            AddDistrictColor1((byte)Mathf.Max(0, this.HighlightDistrict), 255, ref c);
            AddDistrictColor2((byte)Mathf.Max(0, this.HighlightDistrict), DistrictPolicies.Policies.None, 255, true, ref c2);
            areaMaterial.SetColor(ID_Highlight1, c);
            areaMaterial.SetColor(ID_Highlight2, c2);
            if (this.HighlightPolicy != DistrictPolicies.Policies.None)
                areaMaterial.EnableKeyword("POLICYTOOL_ON");
            else
                areaMaterial.DisableKeyword("POLICYTOOL_ON");
            //begin mod
            Bounds bounds = new Bounds(new Vector3(0f, 512f, 0f), new Vector3(19.2f * GRID, 1024f, 19.2f * GRID) + Vector3.one);
            //end mod
            ++Singleton<DistrictManager>.instance.m_drawCallData.m_overlayCalls;
            Singleton<RenderManager>.instance.OverlayEffect.DrawEffect(cameraInfo, areaMaterial, 0, bounds);
        }

        [ReplaceMethod]
        private void UpdateTexture()
        {
            var modifyLock = modifyLockField.GetValue(this);
            do
                ;
            while (!Monitor.TryEnter(modifyLock, SimulationManager.SYNCHRONIZE_TIMEOUT));
            int num;
            int num2;
            int num3;
            int num4;
            bool fullUpdate;
            try
            {
                num = (int)modifiedX1Field.GetValue(this);
                num2 = (int)modifiedZ1Field.GetValue(this);
                num3 = (int)modifiedX2Field.GetValue(this);
                num4 = (int)modifiedZ2Field.GetValue(this);
                fullUpdate = (bool)fullUpdateField.GetValue(this);
                modifiedX1Field.SetValue(this, 10000);
                modifiedZ1Field.SetValue(this, 10000);
                modifiedX2Field.SetValue(this, -10000);
                modifiedZ2Field.SetValue(this, -10000);
                fullUpdateField.SetValue(this, false);
            }
            finally
            {
                Monitor.Exit(modifyLock);
            }
            int[] areaGrid = Singleton<GameAreaManager>.instance.m_areaGrid;
            int num5 = Mathf.RoundToInt(99.99999f);
            //begin mod
            int num6 = (5 * num5 >> 1) - HALFGRID; //TODO(earalov): 5 looks like area manager's grid size. Replace with 9? Or: 5 * 100 = 500~512. Maybe related to district manager's grid?
            //end mod
            if ((num3 - num + 1) * (num4 - num2 + 1) > 65536) //TODO(earalov): 65536 = 256*256. Maybe replace with HALFGRID * HALFGRID?
            {
                num = 1;
                num2 = 1;
                //begin mod
                num3 = GRID - 2;
                num4 = GRID - 2;
                //end mod
                if (fullUpdate)
                {
                    for (int i = num2; i <= num4; i++)
                    {
                        for (int j = num; j <= num3; j++)
                        {
                            //begin mod
                            int num7 = i * GRID + j;
                            DistrictManager.Cell cell = districtGrid[num7];
                            //end mod
                            Color32 color = new Color32(128, 128, 128, 128);
                            AddDistrictColor1(cell.m_district1, cell.m_alpha1, ref color);
                            AddDistrictColor1(cell.m_district2, cell.m_alpha2, ref color);
                            AddDistrictColor1(cell.m_district3, cell.m_alpha3, ref color);
                            AddDistrictColor1(cell.m_district4, cell.m_alpha4, ref color);
                            colorBuffer[num7] = color;
                        }
                    }
                    //begin mod
                    districtTexture1.SetPixels32(colorBuffer);
                    districtTexture1.Apply();
                    //end mod
                }
                for (int k = num2; k <= num4; k++)
                {
                    for (int l = num; l <= num3; l++)
                    {
                        //begin mod
                        int num8 = k * GRID + l;
                        DistrictManager.Cell cell2 = districtGrid[num8];
                        //end mod
                        bool inArea = false;
                        int num9 = (l + num6) / num5;
                        int num10 = (k + num6) / num5;
                        if (num9 >= 0 && num9 < 5 && num10 >= 0 && num10 < 5)  //TODO(earalov): 5 looks like area manager's grid size. Replace with 9?
                            inArea = (areaGrid[num10 * 5 + num9] != 0);        //TODO(earalov): 5 looks like area manager's grid size. Replace with 9?
                        Color32 color2 = new Color32(128, 128, 128, 128);
                        AddDistrictColor2(cell2.m_district1, this.HighlightPolicy, cell2.m_alpha1, inArea, ref color2);
                        AddDistrictColor2(cell2.m_district2, this.HighlightPolicy, cell2.m_alpha2, inArea, ref color2);
                        AddDistrictColor2(cell2.m_district3, this.HighlightPolicy, cell2.m_alpha3, inArea, ref color2);
                        AddDistrictColor2(cell2.m_district4, this.HighlightPolicy, cell2.m_alpha4, inArea, ref color2);
                        //begin mod
                        colorBuffer[num8] = color2;
                        //end mod
                    }
                }
                //begin mod
                districtTexture2.SetPixels32(colorBuffer);
                districtTexture2.Apply();
                //end mod
            }
            else
            {
                num = Mathf.Max(1, num);
                num2 = Mathf.Max(1, num2);
                //begin mod
                num3 = Mathf.Min(GRID - 2, num3);
                num4 = Mathf.Min(GRID - 2, num4);
                //end mod
                for (int m = num2; m <= num4; m++)
                {
                    for (int n = num; n <= num3; n++)
                    {
                        //begin mod
                        int num11 = m * GRID + n;
                        DistrictManager.Cell cell3 = districtGrid[num11];
                        //end mod
                        if (fullUpdate)
                        {
                            Color32 c = new Color32(128, 128, 128, 128);
                            AddDistrictColor1(cell3.m_district1, cell3.m_alpha1, ref c);
                            AddDistrictColor1(cell3.m_district2, cell3.m_alpha2, ref c);
                            AddDistrictColor1(cell3.m_district3, cell3.m_alpha3, ref c);
                            AddDistrictColor1(cell3.m_district4, cell3.m_alpha4, ref c);
                            districtTexture1.SetPixel(n, m, c);
                        }
                        bool inArea2 = false;
                        int num12 = (n + num6) / num5;
                        int num13 = (m + num6) / num5;
                        if (num12 >= 0 && num12 < 5 && num13 >= 0 && num13 < 5)  //TODO(earalov): 5 looks like area manager's grid size. Replace with 9?
                            inArea2 = (areaGrid[num13 * 5 + num12] != 0);        //TODO(earalov): 5 looks like area manager's grid size. Replace with 9?
                        Color32 c2 = new Color32(128, 128, 128, 128);
                        AddDistrictColor2(cell3.m_district1, this.HighlightPolicy, cell3.m_alpha1, inArea2, ref c2);
                        AddDistrictColor2(cell3.m_district2, this.HighlightPolicy, cell3.m_alpha2, inArea2, ref c2);
                        AddDistrictColor2(cell3.m_district3, this.HighlightPolicy, cell3.m_alpha3, inArea2, ref c2);
                        AddDistrictColor2(cell3.m_district4, this.HighlightPolicy, cell3.m_alpha4, inArea2, ref c2);
                        //begin mod
                        districtTexture2.SetPixel(n, m, c2);
                        //end mod
                    }
                }
                //begin mod
                if (fullUpdate)
                    districtTexture1.Apply();
                districtTexture2.Apply();
                //end mod
            }
        }

        //no changes
        private void AddDistrictColor1(byte district, byte alpha, ref Color32 color1)
        {
            color1.r = ((int)district & 1) == 0 ? (byte)Mathf.Min((int)color1.r, (int)byte.MaxValue - (int)alpha) : (byte)Mathf.Max((int)color1.r, (int)alpha);
            color1.g = ((int)district & 2) == 0 ? (byte)Mathf.Min((int)color1.g, (int)byte.MaxValue - (int)alpha) : (byte)Mathf.Max((int)color1.g, (int)alpha);
            color1.b = ((int)district & 4) == 0 ? (byte)Mathf.Min((int)color1.b, (int)byte.MaxValue - (int)alpha) : (byte)Mathf.Max((int)color1.b, (int)alpha);
            if (((int)district & 8) != 0)
                color1.a = (byte)Mathf.Max((int)color1.a, (int)alpha);
            else
                color1.a = (byte)Mathf.Min((int)color1.a, (int)byte.MaxValue - (int)alpha);
        }

        //no changes
        private void AddDistrictColor2(byte district, DistrictPolicies.Policies policy, byte alpha, bool inArea, ref Color32 color2)
        {
            color2.r = ((int)district & 16) == 0 ? (byte)Mathf.Min((int)color2.r, (int)byte.MaxValue - (int)alpha) : (byte)Mathf.Max((int)color2.r, (int)alpha);
            color2.g = ((int)district & 32) == 0 ? (byte)Mathf.Min((int)color2.g, (int)byte.MaxValue - (int)alpha) : (byte)Mathf.Max((int)color2.g, (int)alpha);
            color2.b = ((int)district & 64) == 0 ? (byte)Mathf.Min((int)color2.b, (int)byte.MaxValue - (int)alpha) : (byte)Mathf.Max((int)color2.b, (int)alpha);
            if (policy != DistrictPolicies.Policies.None)
            {
                if (this.m_districts.m_buffer[(int)district].IsPolicySet(policy) && (inArea || (int)district != 0))
                    color2.a = (byte)Mathf.Max((int)color2.a, (int)alpha);
                else
                    color2.a = (byte)Mathf.Min((int)color2.a, (int)byte.MaxValue - (int)alpha);
            }
            else
                color2.a = (byte)Mathf.Min((int)color2.a, (int)byte.MaxValue - (int)alpha);
        }

        [ReplaceMethod]
        public new void NamesModified()
        {
            //begin mod
            int num = distanceBuffer.Length;
            for (int i = 0; i < num; i++)
                distanceBuffer[i] = 0;
            //end mod
            for (int j = 0; j < 128; j++)
                //begin mod
                m_tempData[j] = default(TempDistrictData);
            //end mod
            int num2 = 2;
            //begin mod
            int num3 = GRID * 2;
            //end mod
            int num4 = 0;
            int num5 = 0;
            //begin mod
            for (int k = 0; k < HALFGRID; k++)
            {
                for (int l = 0; l < HALFGRID; l++)
                {
                    //end mod
                    int num6 = k * num3 + l * num2;
                    //begin mod
                    byte district = districtGrid[num6].m_district1;
                    if (district != 0 && (l == 0 || k == 0 || l == HALFGRID - 1 || k == HALFGRID - 1 || districtGrid[num6 - num3].m_district1 != district || districtGrid[num6 - num2].m_district1 != district || districtGrid[num6 + num2].m_district1 != district || districtGrid[num6 + num3].m_district1 != district))
                    {
                        int num7 = k * HALFGRID + l;
                        distanceBuffer[num7] = 1;
                        indexBuffer[num5] = num7;
                        num5 = ((num5 + 1) % (HALFGRID * HALFGRID));
                        //end mod
                        m_tempData[(int)district].m_averageX = m_tempData[(int)district].m_averageX + l;
                        m_tempData[(int)district].m_averageZ = m_tempData[(int)district].m_averageZ + k;
                        ++m_tempData[(int)district].m_divider;
                    }
                }
            }
            for (int m = 0; m < 128; m++)
            {
                int divider = m_tempData[m].m_divider;
                if (divider != 0)
                {
                    m_tempData[m].m_averageX = (m_tempData[m].m_averageX + divider >> 1) / divider;
                    m_tempData[m].m_averageZ = (m_tempData[m].m_averageZ + divider >> 1) / divider;
                }
            }
            while (num4 != num5)
            {
                int num8 = indexBuffer[num4];
                num4 = ((num4 + 1) % (HALFGRID * HALFGRID));
                int num9 = num8 % HALFGRID;
                int num10 = num8 / HALFGRID;
                int num11 = num10 * num3 + num9 * num2;
                //begin mod
                byte district2 = districtGrid[num11].m_district1;
                //end mod
                int num12 = num9 - m_tempData[(int)district2].m_averageX;
                int num13 = num10 - m_tempData[(int)district2].m_averageZ;
                //begin mod
                int num14 = (GRID * GRID) - (GRID * GRID / 2) / distanceBuffer[num8] - num12 * num12 - num13 * num13;
                //end mod
                if (num14 > m_tempData[(int)district2].m_bestScore)
                {
                    m_tempData[(int)district2].m_bestScore = num14;
                    m_tempData[(int)district2].m_bestLocation = num8;
                }
                int num15 = num8 - 1;
                //begin mod
                if (num9 > 0 && distanceBuffer[num15] == 0 && districtGrid[num11 - num2].m_district1 == district2)
                {
                    distanceBuffer[num15] = (distanceBuffer[num8] + 1);
                    indexBuffer[num5] = num15;
                    num5 = ((num5 + 1) % (HALFGRID * HALFGRID));
                }
                //end mod
                num15 = num8 + 1;
                //begin mod
                if (num9 < HALFGRID - 1 && distanceBuffer[num15] == 0 && districtGrid[num11 + num2].m_district1 == district2)
                {
                    distanceBuffer[num15] = (distanceBuffer[num8] + 1);
                    indexBuffer[num5] = num15;
                    num5 = ((num5 + 1) % (HALFGRID * HALFGRID));
                }
                num15 = num8 - HALFGRID;
                if (num10 > 0 && distanceBuffer[num15] == 0 && districtGrid[num11 - num3].m_district1 == district2)
                {
                    distanceBuffer[num15] = (distanceBuffer[num8] + 1);
                    indexBuffer[num5] = num15;
                    num5 = ((num5 + 1) % (HALFGRID * HALFGRID));
                }
                num15 = num8 + HALFGRID;
                if (num10 < HALFGRID - 1 && distanceBuffer[num15] == 0 && districtGrid[num11 + num3].m_district1 == district2)
                {
                    distanceBuffer[num15] = (distanceBuffer[num8] + 1);
                    indexBuffer[num5] = num15;
                    num5 = ((num5 + 1) % (HALFGRID * HALFGRID));
                }
                //end mod
            }
            for (int n = 0; n < 128; n++)
            {
                int bestLocation = m_tempData[n].m_bestLocation;
                Vector3 vector;
                //begin mod
                vector.x = 19.2f * (float)(bestLocation % HALFGRID) * 2f - 19.2f * HALFGRID;
                //end mod
                vector.y = 0f;
                //begin mod
                vector.z = 19.2f * (float)(bestLocation / HALFGRID) * 2f - 19.2f * HALFGRID;
                //end
                vector.y = Singleton<TerrainManager>.instance.SampleRawHeightSmoothWithWater(vector, false, 0f);
                this.m_districts.m_buffer[n].m_nameLocation = vector;
            }
            namesModifiedField.SetValue(this, true);
        }

        [ReplaceMethod]
        public new void GetDistrictArea(byte district, out int minX, out int minZ, out int maxX, out int maxZ)
        {
            minX = 10000;
            minZ = 10000;
            maxX = -10000;
            maxZ = -10000;
            //begin mod
            for (int index1 = 0; index1 < GRID; ++index1)
            {
                for (int index2 = 0; index2 < GRID; ++index2)
                {
                    DistrictManager.Cell cell = districtGrid[index1 * GRID + index2];
                    //end mod
                    if ((int)cell.m_alpha1 != 0 && (int)cell.m_district1 == (int)district)
                    {
                        if (index2 < minX)
                            minX = index2;
                        if (index1 < minZ)
                            minZ = index1;
                        if (index2 > maxX)
                            maxX = index2;
                        if (index1 > maxZ)
                            maxZ = index1;
                    }
                    else if ((int)cell.m_alpha2 != 0 && (int)cell.m_district2 == (int)district)
                    {
                        if (index2 < minX)
                            minX = index2;
                        if (index1 < minZ)
                            minZ = index1;
                        if (index2 > maxX)
                            maxX = index2;
                        if (index1 > maxZ)
                            maxZ = index1;
                    }
                    else if ((int)cell.m_alpha3 != 0 && (int)cell.m_district3 == (int)district)
                    {
                        if (index2 < minX)
                            minX = index2;
                        if (index1 < minZ)
                            minZ = index1;
                        if (index2 > maxX)
                            maxX = index2;
                        if (index1 > maxZ)
                            maxZ = index1;
                    }
                    else if ((int)cell.m_alpha4 != 0 && (int)cell.m_district4 == (int)district)
                    {
                        if (index2 < minX)
                            minX = index2;
                        if (index1 < minZ)
                            minZ = index1;
                        if (index2 > maxX)
                            maxX = index2;
                        if (index1 > maxZ)
                            maxZ = index1;
                    }
                }
            }
        }

        //TODO(earalov): make sure this method doesn't get inlined
        [ReplaceMethod]
        public new byte GetDistrict(int x, int z)
        {
            //begin mod
            return districtGrid[z * GRID + x].m_district1;
            //end mod
        }

        [ReplaceMethod]
        public byte GetDistrict(Vector3 worldPos)
        {
            //begin mod
            int num = Mathf.Clamp((int)(worldPos.x / 19.2f + HALFGRID), 0, GRID - 1);
            int num2 = Mathf.Clamp((int)(worldPos.z / 19.2f + HALFGRID), 0, GRID - 1);
            int num3 = num2 * GRID + num;
            return districtGrid[num3].m_district1;
            //end mod
        }

        [ReplaceMethod]
        public new byte SampleDistrict(Vector3 worldPos)
        {
            //begin mod
            int num = Mathf.RoundToInt(worldPos.x * 13.333333f + (HALFGRID * HALFGRID) - HALFGRID);
            int num2 = Mathf.RoundToInt(worldPos.z * 13.333333f + (HALFGRID * HALFGRID) - HALFGRID);
            int num3 = Mathf.Clamp((int)(worldPos.x / 19.2f + HALFGRID), 0, GRID - 1);
            int num4 = Mathf.Clamp((int)(worldPos.z / 19.2f + HALFGRID), 0, GRID - 1);
            int num5 = Mathf.Min(num3 + 1, GRID - 1);
            int num6 = Mathf.Min(num4 + 1, GRID - 1);
            //end mod
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            int num10 = 0;
            int num11 = 0;
            int num12 = 0;
            int num13 = 0;
            //begin mod
            SetBitAlphas(districtGrid[num4 * GRID + num3], (255 - (num & 255)) * (255 - (num2 & 255)), ref num7, ref num8, ref num9, ref num10, ref num11, ref num12, ref num13);
            SetBitAlphas(districtGrid[num4 * GRID + num5], (num & 255) * (255 - (num2 & 255)), ref num7, ref num8, ref num9, ref num10, ref num11, ref num12, ref num13);
            SetBitAlphas(districtGrid[num6 * GRID + num3], (255 - (num & 255)) * (num2 & 255), ref num7, ref num8, ref num9, ref num10, ref num11, ref num12, ref num13);
            SetBitAlphas(districtGrid[num6 * GRID + num5], (num & 255) * (num2 & 255), ref num7, ref num8, ref num9, ref num10, ref num11, ref num12, ref num13);
            //end mod
            byte b = 0;
            if (num7 > 0)
            {
                b |= 1;
            }
            if (num8 > 0)
            {
                b |= 2;
            }
            if (num9 > 0)
            {
                b |= 4;
            }
            if (num10 > 0)
            {
                b |= 8;
            }
            if (num11 > 0)
            {
                b |= 16;
            }
            if (num12 > 0)
            {
                b |= 32;
            }
            if (num13 > 0)
            {
                b |= 64;
            }
            return b;
        }

        //no changes
        private void SetBitAlphas(DistrictManager.Cell cell, int alpha, ref int b1, ref int b2, ref int b3, ref int b4, ref int b5, ref int b6, ref int b7)
        {
            int b1_1 = 0;
            int b2_1 = 0;
            int b3_1 = 0;
            int b4_1 = 0;
            int b5_1 = 0;
            int b6_1 = 0;
            int b7_1 = 0;
            this.SetBitAlphas((int)cell.m_district1, (int)cell.m_alpha1, ref b1_1, ref b2_1, ref b3_1, ref b4_1, ref b5_1, ref b6_1, ref b7_1);
            this.SetBitAlphas((int)cell.m_district2, (int)cell.m_alpha2, ref b1_1, ref b2_1, ref b3_1, ref b4_1, ref b5_1, ref b6_1, ref b7_1);
            this.SetBitAlphas((int)cell.m_district3, (int)cell.m_alpha3, ref b1_1, ref b2_1, ref b3_1, ref b4_1, ref b5_1, ref b6_1, ref b7_1);
            this.SetBitAlphas((int)cell.m_district4, (int)cell.m_alpha4, ref b1_1, ref b2_1, ref b3_1, ref b4_1, ref b5_1, ref b6_1, ref b7_1);
            b1 = b1 + b1_1 * alpha;
            b2 = b2 + b2_1 * alpha;
            b3 = b3 + b3_1 * alpha;
            b4 = b4 + b4_1 * alpha;
            b5 = b5 + b5_1 * alpha;
            b6 = b6 + b6_1 * alpha;
            b7 = b7 + b7_1 * alpha;
        }

        //no changes
        private void SetBitAlphas(int district, int alpha, ref int b1, ref int b2, ref int b3, ref int b4, ref int b5, ref int b6, ref int b7)
        {
            b1 = (district & 1) == 0 ? Mathf.Min(b1, 128 - alpha) : Mathf.Max(b1, alpha - 128);
            b2 = (district & 2) == 0 ? Mathf.Min(b2, 128 - alpha) : Mathf.Max(b2, alpha - 128);
            b3 = (district & 4) == 0 ? Mathf.Min(b3, 128 - alpha) : Mathf.Max(b3, alpha - 128);
            b4 = (district & 8) == 0 ? Mathf.Min(b4, 128 - alpha) : Mathf.Max(b4, alpha - 128);
            b5 = (district & 16) == 0 ? Mathf.Min(b5, 128 - alpha) : Mathf.Max(b5, alpha - 128);
            b6 = (district & 32) == 0 ? Mathf.Min(b6, 128 - alpha) : Mathf.Max(b6, alpha - 128);
            if ((district & 64) != 0)
                b7 = Mathf.Max(b7, alpha - 128);
            else
                b7 = Mathf.Min(b7, 128 - alpha);
        }

        [ReplaceMethod]
        public void ModifyCell(int x, int z, DistrictManager.Cell cell)
        {
            if ((int)cell.m_alpha2 > (int)cell.m_alpha1)
                this.Exchange(ref cell.m_alpha1, ref cell.m_alpha2, ref cell.m_district1, ref cell.m_district2);
            if ((int)cell.m_alpha3 > (int)cell.m_alpha1)
                this.Exchange(ref cell.m_alpha1, ref cell.m_alpha3, ref cell.m_district1, ref cell.m_district3);
            if ((int)cell.m_alpha4 > (int)cell.m_alpha1)
                this.Exchange(ref cell.m_alpha1, ref cell.m_alpha4, ref cell.m_district1, ref cell.m_district4);
            //begin mod
            int index = z * GRID + x;
            DistrictManager.Cell cell1 = districtGrid[index];
            districtGrid[index] = cell;
            //end mod
            this.m_districts.m_buffer[(int)cell.m_district1].m_totalAlpha += (uint)cell.m_alpha1;
            this.m_districts.m_buffer[(int)cell.m_district2].m_totalAlpha += (uint)cell.m_alpha2;
            this.m_districts.m_buffer[(int)cell.m_district3].m_totalAlpha += (uint)cell.m_alpha3;
            this.m_districts.m_buffer[(int)cell.m_district4].m_totalAlpha += (uint)cell.m_alpha4;
            this.EraseDistrict(cell1.m_district1, ref this.m_districts.m_buffer[(int)cell1.m_district1], (uint)cell1.m_alpha1);
            this.EraseDistrict(cell1.m_district2, ref this.m_districts.m_buffer[(int)cell1.m_district2], (uint)cell1.m_alpha2);
            this.EraseDistrict(cell1.m_district3, ref this.m_districts.m_buffer[(int)cell1.m_district3], (uint)cell1.m_alpha3);
            this.EraseDistrict(cell1.m_district4, ref this.m_districts.m_buffer[(int)cell1.m_district4], (uint)cell1.m_alpha4);
        }

        //no changes
        private void Exchange(ref byte alpha1, ref byte alpha2, ref byte district1, ref byte district2)
        {
            byte num1 = alpha2;
            byte num2 = district2;
            alpha2 = alpha1;
            district2 = district1;
            alpha1 = num1;
            district1 = num2;
        }

        //no changes
        private void EraseDistrict(byte district, ref District data, uint amount)
        {
            if (amount >= data.m_totalAlpha)
            {
                if ((int)district == 0)
                    data.m_totalAlpha = 0U;
                else
                    this.ReleaseDistrictImplementation(district, ref this.m_districts.m_buffer[(int)district]);
            }
            else
                data.m_totalAlpha -= amount;
        }

        //this is called with reflection to prevent conflict with BuildingThemes
        private void ReleaseDistrictImplementation(byte district, ref District data)
        {
            var args = new object[]
            {
                district,
                data
            };
            ReleaseDistrictImplementationMethod.Invoke(this, args);
            data = (District)args[1];
        }
    }
}
