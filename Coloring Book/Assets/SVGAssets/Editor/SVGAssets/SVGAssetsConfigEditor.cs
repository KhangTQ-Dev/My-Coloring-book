/****************************************************************************
** Copyright (c) 2013-2018 Mazatech S.r.l.
** All rights reserved.
** 
** Redistribution and use in source and binary forms, with or without
** modification, are permitted (subject to the limitations in the disclaimer
** below) provided that the following conditions are met:
** 
** - Redistributions of source code must retain the above copyright notice,
**   this list of conditions and the following disclaimer.
** 
** - Redistributions in binary form must reproduce the above copyright notice,
**   this list of conditions and the following disclaimer in the documentation
**   and/or other materials provided with the distribution.
** 
** - Neither the name of Mazatech S.r.l. nor the names of its contributors
**   may be used to endorse or promote products derived from this software
**   without specific prior written permission.
** 
** NO EXPRESS OR IMPLIED LICENSES TO ANY PARTY'S PATENT RIGHTS ARE GRANTED
** BY THIS LICENSE. THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
** CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT
** NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
** A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER
** OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
** EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
** PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
** OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
** WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
** OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
** ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
** 
** For any information, please contact info@mazatech.com
** 
****************************************************************************/
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
    using UnityEditor;
    using System.IO;
#endif
using UnityEngine;
using UnityEngine.UIElements;

// Create SVGAssetsSettingsProvider by deriving from SettingsProvider
class SVGAssetsConfigProvider : SettingsProvider
{
    private SerializedObject m_SerializedConfig;

    public SVGAssetsConfigProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope)
    {
    }

    public override void OnActivate(string searchContext, VisualElement rootElement)
    {
        // this function is called when the user clicks on the SVGAsstes element in the Settings window.
        m_SerializedConfig = SVGAssetsConfigUnityScriptable.GetSerializedConfig();
    }

    private bool IsValidFontAsset(object obj)
    {
        bool ok = obj is TextAsset;

    #if UNITY_EDITOR
        if (ok)
        {
            TextAsset txtAsset = obj as TextAsset;
            // check file extension
            string assetPath = AssetDatabase.GetAssetPath(txtAsset);
            string fileExt = Path.GetExtension(assetPath);
            // if must be .bytes (see SVGRenamerImporter.cs)
            ok = fileExt.Equals(".bytes", System.StringComparison.OrdinalIgnoreCase);
        }
    #endif

        return ok;
    }

    private bool DrawFontResource(SVGAssetsConfigUnity config,
                                  int index,
                                  out Rect rowRect)
    {
        bool isDirty = false;
        SVGAssetsConfigUnity.SVGFontResourceUnity fontResource = config.GetFontResource(index) as SVGAssetsConfigUnity.SVGFontResourceUnity;
        bool highlight = (_dragInfo.Dragging && (_dragInfo.DraggedObject == fontResource)) ? true : false;

        if ((_dragInfo.InsertIdx == index) && _dragInfo.InsertBefore)
        {
            // draw a separator before the row
            GUILayout.Box(GUIContent.none, Styles.BlueLine, GUILayout.ExpandWidth(true), GUILayout.Height(2));
        }

        // if the font row is the dragged one, change colors
        if (highlight)
        {
            EditorGUILayout.BeginHorizontal(Styles.HighlightRow);
            EditorGUILayout.LabelField(fontResource.FontAsset.name, Styles.HighlightRow, GUILayout.MinWidth(10));
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fontResource.FontAsset.name, GUILayout.MinWidth(10));
        }

        // font family, weight, stretch, style
        string family = EditorGUILayout.TextField(fontResource.Family, GUILayout.MinWidth(100));
        SVGFontWeight weight = (SVGFontWeight)EditorGUILayout.EnumPopup(fontResource.Weight, GUILayout.MaxWidth(130));
        SVGFontStretch stretch = (SVGFontStretch)EditorGUILayout.EnumPopup(fontResource.Stretch);
        SVGFontStyle style = (SVGFontStyle)EditorGUILayout.EnumPopup(fontResource.Style, GUILayout.MaxWidth(100));

        // if 'Remove' button is clicked, remove the font entry
        if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(70)))
        {
            config.FontRemove(index);
            isDirty = true;
        }
        EditorGUILayout.EndHorizontal();

        rowRect = GUILayoutUtility.GetLastRect();

        if ((_dragInfo.InsertIdx == index) && (!_dragInfo.InsertBefore))
        {
            // draw a separator after the row
            GUILayout.Box(GUIContent.none, Styles.BlueLine, GUILayout.ExpandWidth(true), GUILayout.Height(2));
        }

        // update font parameters
        if (family != fontResource.Family)
        {
            fontResource.Family = family;
            isDirty = true;
        }
        if (weight != fontResource.Weight)
        {
            fontResource.Weight = weight;
            isDirty = true;
        }
        if (stretch != fontResource.Stretch)
        {
            fontResource.Stretch = stretch;
            isDirty = true;
        }
        if (style != fontResource.Style)
        {
            fontResource.Style = style;
            isDirty = true;
        }

        return isDirty;
    }

    private bool DrawFontResources(SVGAssetsConfigUnity config,
                                   Event currentEvent,
                                   out Rect scollRect)
    {
        bool isDirty = false;

        // keep track of drawn rows
        if (currentEvent.type != EventType.Layout)
        {
            _fontsAssetsRects = new List<Rect>();
        }

        Vector2 scrollPos = EditorGUILayout.BeginScrollView(_fontsListScrollPos, GUILayout.ExpandWidth(true));
        for (int i = 0; i < config.FontsCount(); ++i)
        {
            isDirty |= DrawFontResource(config, i, out Rect rowRect);
            // keep track of row rectangle
            if (currentEvent.type != EventType.Layout)
            {
                _fontsAssetsRects.Add(rowRect);
            }
        }
        EditorGUILayout.EndScrollView();

        // keep track of the scrollview area
        scollRect = GUILayoutUtility.GetLastRect();

        if (_fontsListScrollPos != scrollPos)
        {
            _fontsListScrollPos = scrollPos;
        }

        return isDirty;
    }

    private bool HandleDragEvents(SVGAssetsConfigUnity config,
                                  Event currentEvent,
                                  Rect scrollRect)
    {
        int i;
        bool isDirty = false;

        // events handler
        if (currentEvent.type != EventType.Layout)
        {
            bool needRepaint = false;
            // get mouse position relative to scollRect
            Vector2 mousePos = currentEvent.mousePosition - new Vector2(scrollRect.xMin, scrollRect.yMin);

            if (scrollRect.Contains(currentEvent.mousePosition))
            {
                bool separatorInserted = false;

                for (i = 0; i < config.FontsCount(); ++i)
                {
                    // get the row rectangle relative to i-thm font
                    Rect rowRect = _fontsAssetsRects[i];
                    // expand the rectangle height
                    rowRect.yMin -= 3;
                    rowRect.yMax += 3;

                    if (rowRect.Contains(mousePos))
                    {
                        // a mousedown on a row, will stop an already started drag operation
                        if (currentEvent.type == EventType.MouseDown)
                        {
                            _dragInfo.StopDrag();
                        }
                        // check if we are already dragging an object
                        if (_dragInfo.Dragging)
                        {
                            if (!separatorInserted)
                            {
                                bool ok = true;
                                bool dragBefore = (mousePos.y <= (rowRect.yMin + (rowRect.height / 2))) ? true : false;
                                // if we are dragging a text (asset) file, all positions are ok
                                // if we are dragging an already present SVG row, we must perform additional checks
                                if (!IsValidFontAsset(_dragInfo.DraggedObject))
                                {
                                    if (_dragInfo.DraggedObject == config.GetFontResource(i))
                                    {
                                        ok = false;
                                    }
                                    else
                                    {
                                        if (dragBefore)
                                        {
                                            if ((i > 0) && (_dragInfo.DraggedObject == config.GetFontResource(i - 1)))
                                            {
                                                ok = false;
                                            }
                                        }
                                        else
                                        {
                                            if (i < (config.FontsCount() - 1) && (_dragInfo.DraggedObject == config.GetFontResource(i + 1)))
                                            {
                                                ok = false;
                                            }
                                        }
                                    }
                                }

                                if (ok)
                                {
                                    if (dragBefore)
                                    {
                                        _dragInfo.InsertIdx = i;
                                        _dragInfo.InsertBefore = true;
                                        separatorInserted = true;
                                    }
                                    else
                                    {
                                        _dragInfo.InsertIdx = i;
                                        _dragInfo.InsertBefore = false;
                                        separatorInserted = true;
                                    }
                                    needRepaint = true;
                                }
                            }
                        }
                        else
                        {
                            // initialize the drag of an already present SVG document
                            if (currentEvent.type == EventType.MouseDrag)
                            {
                                DragAndDrop.PrepareStartDrag();
                                DragAndDrop.StartDrag("Start drag");
                                _dragInfo.StartDrag(config.GetFontResource(i));
                                needRepaint = true;
                            }
                        }
                    }
                }

                // mouse is dragging inside the drop box, but not under an already present row; insertion point is inside the last element
                if (_dragInfo.Dragging && (!separatorInserted) && (config.FontsCount() > 0) && (mousePos.y > _fontsAssetsRects[config.FontsCount() - 1].yMax))
                {
                    bool ok = true;

                    if (!IsValidFontAsset(_dragInfo.DraggedObject))
                    {
                        if (_dragInfo.DraggedObject == config.GetFontResource(config.FontsCount() - 1))
                        {
                            ok = false;
                        }
                    }

                    if (ok)
                    {
                        _dragInfo.InsertIdx = config.FontsCount() - 1;
                        _dragInfo.InsertBefore = false;
                        needRepaint = true;
                    }
                }
            }
            else
            {
                _dragInfo.InsertIdx = -1;
            }

            if (needRepaint)
            {
                Repaint();
            }
        }

        if (currentEvent.type == EventType.DragExited)
        {
            _dragInfo.StopDrag();
            DragAndDrop.objectReferences = Array.Empty<UnityEngine.Object>();
        }
        else
        {
            switch (currentEvent.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (_dragInfo.Dragging)
                    {
                        bool dragValid = true;

                        if (scrollRect.Contains(currentEvent.mousePosition) && dragValid)
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                            if (currentEvent.type == EventType.DragPerform)
                            {
                                int index;

                                // accept drag&drop operation
                                DragAndDrop.AcceptDrag();
                                // check if we are dropping a text asset
                                if (IsValidFontAsset(_dragInfo.DraggedObject))
                                {
                                    // if a valid inter-position has not been selected, append the new asset at the end of list
                                    if (_dragInfo.InsertIdx < 0)
                                    {
                                        index = config.FontsCount();
                                    }
                                    else
                                    {
                                        index = _dragInfo.InsertBefore ? _dragInfo.InsertIdx : (_dragInfo.InsertIdx + 1);
                                    }
                                    // add the text asset to the SVG list
                                    if (config.FontAdd(_dragInfo.DraggedObject as TextAsset, index))
                                    {
                                        isDirty = true;
                                    }
                                }
                                else
                                {
                                    // we are dropping an already present SVG row
                                    index = _dragInfo.InsertBefore ? _dragInfo.InsertIdx : (_dragInfo.InsertIdx + 1);
                                    if (config.FontMove(_dragInfo.DraggedObject as SVGAssetsConfigUnity.SVGFontResourceUnity, index))
                                    {
                                        isDirty = true;
                                    }
                                }
                                // now we can close the drag operation
                                _dragInfo.StopDrag();
                            }
                        }
                        else
                        {
                            // if we are dragging outside of the allowed drop region, simply reject the drag&drop
                            DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                        }
                    }
                    else
                    {
                        if (scrollRect.Contains(currentEvent.mousePosition))
                        {
                            if ((DragAndDrop.objectReferences != null) && (DragAndDrop.objectReferences.Length > 0))
                            {
                                UnityEngine.Object draggedObject = DragAndDrop.objectReferences[0];
                                // check object type, only TextAssets are allowed
                                if (IsValidFontAsset(draggedObject))
                                {
                                    _dragInfo.StartDrag(DragAndDrop.objectReferences[0]);
                                    Repaint();
                                }
                                else
                                {
                                    // acceptance is not confirmed (e.g. we are dragging a binary file)
                                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                                }
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        return isDirty;
    }

    private bool DrawInspector(SVGAssetsConfigUnity config)
    {
        bool isDirty = false;
        // get current event
        Event currentEvent = Event.current;

        GUILayout.Space(10);

        // user-agent language settings
        GUILayout.Space(10);
        EditorGUILayout.LabelField("User-agent language settings", EditorStyles.boldLabel);
        GUILayout.Space(2);
        EditorGUIUtility.labelWidth = 70;
        EditorGUILayout.BeginHorizontal();
        string language = EditorGUILayout.TextField(Styles.Language, config.Language, GUILayout.MaxWidth(190));
        GUILayout.Space(10);
        string script = EditorGUILayout.TextField(Styles.Script, config.Script, GUILayout.MaxWidth(190));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        string region = EditorGUILayout.TextField(Styles.Region, config.Region, GUILayout.MaxWidth(190));
        GUILayout.Space(10);
        string variant = EditorGUILayout.TextField(Styles.Variant, config.Variant, GUILayout.MaxWidth(190));
        EditorGUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = 0;

        GUILayout.Space(20);

        // curves quality
        EditorGUILayout.LabelField("Rendering quality settings", EditorStyles.boldLabel);
        GUILayout.Space(2);
        float curvesQuality = EditorGUILayout.Slider(Styles.CurvesQuality, config.CurvesQuality, 1.0f, 100.0f);

        GUILayout.Space(20);

        // log facility
        EditorGUILayout.LabelField("Log facility", EditorStyles.boldLabel);
        GUILayout.Space(2);
        EditorGUILayout.BeginHorizontal();
        SVGLogLevel logLevel = (SVGLogLevel)EditorGUILayout.EnumFlagsField(Styles.LogLevel, config.LogLevelEditor);
        GUILayout.Space(10);
        // max 1M log capacity
        int logCapacity = EditorGUILayout.IntSlider(Styles.LogCapacity, (int)config.LogCapacity, 0, 999999);
        EditorGUILayout.EndHorizontal();

        // draw the list of font resources
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Font resources (drag TTF/OTF font files here)", EditorStyles.boldLabel);
        isDirty |= DrawFontResources(config, currentEvent, out Rect scrollRect);

        // events handler
        isDirty |= HandleDragEvents(config, currentEvent, scrollRect);

        // update user-agent language settings
        if (language != config.Language)
        {
            // language
            if (SVGAssetsConfig.IsLanguageValid(language))
            {
                config.Language = language;
                isDirty = true;
            }
        }
        if (script != config.Script)
        {
            // script
            if (SVGAssetsConfig.IsScriptValid(script))
            {
                config.Script = script;
                isDirty = true;
            }
        }
        if (region != config.Region)
        {
            // region
            if (SVGAssetsConfig.IsRegionValid(region))
            {
                config.Region = region;
                isDirty = true;
            }
        }
        if (variant != config.Variant)
        {
            // variant
            config.Variant = variant;
            isDirty = true;
        }

        // update curves quality
        if (curvesQuality != config.CurvesQuality)
        {
            config.CurvesQuality = curvesQuality;
            isDirty = true;
        }

        // update log facility
        if (logLevel != config.LogLevelEditor)
        {
            config.LogLevelEditor = logLevel;
            isDirty = true;
        }
        if (logCapacity != config.LogCapacity)
        {
            config.LogCapacity = (uint)logCapacity;
            isDirty = true;
        }

        return isDirty;
    }

    public override void OnGUI(string searchContext)
    {
        // get the real config parameters
        SVGAssetsConfigUnityScriptable configSciptable = m_SerializedConfig.targetObject as SVGAssetsConfigUnityScriptable;

        if (configSciptable != null)
        {
            // enable / disable the GUI
            GUI.enabled = !Application.isPlaying;

            // draw the GUI, and check if the edited configuration is "dirty"
            if (DrawInspector(configSciptable.Config))
            {
                SVGUtils.MarkObjectDirty(configSciptable);
                m_SerializedConfig.ApplyModifiedProperties();
            }
        }
    }

    // Register the SettingsProvider
    [SettingsProvider]
    public static SettingsProvider CreateSVGAssetsConfigProvider()
    {
        return new SVGAssetsConfigProvider("Project/SVGAssets", SettingsScope.Project);
    }

    static private class Styles
    {
        static Styles()
        {
            // blue line separator
            BlueLine = new GUIStyle();
            BlueLine.normal.background = SVGUtils.ColorTexture(new Color32(51, 81, 226, 255));
            // blue highlighted background
            HighlightRow = new GUIStyle();
            HighlightRow.normal.background = SVGUtils.ColorTexture(new Color32(65, 92, 150, 255));
            HighlightRow.normal.textColor = Color.white;
        }

        // blue line separator
        public static GUIStyle BlueLine;
        // blue highlighted background
        public static GUIStyle HighlightRow;
        // Curves quality.
        public static GUIContent CurvesQuality = new GUIContent("Curves quality", "Used by AmanithSVG geometric kernel to approximate curves with straight line segments (flattening). Valid range is [1; 100], where 100 represents the best quality");
        // User-agent language settings.
        public static GUIContent Language = new GUIContent("Language", "2 or 3 alpha characters (shortest ISO 639 code); RFC5646 specifications");
        public static GUIContent Script = new GUIContent("Script", "Empty or a 4 alpha characters (ISO 15924 code); RFC5646 specifications");
        public static GUIContent Region = new GUIContent("Region", "Empty or a 2 alpha or 3 digit characters (ISO 3166-1 code or UN M.49 code); RFC5646 specifications");
        public static GUIContent Variant = new GUIContent("Variant", "Empty or a registered variant; RFC5646 specifications");
        // Log facility.
        public static GUIContent LogLevel = new GUIContent("Log level", "Log level");
        public static GUIContent LogCapacity = new GUIContent("Log capacity (chars)", "Size of log buffer, in characters; if zero is specified, logging is disabled");
    }

    // Keep track of drawn rows (each font file is displayed as a row consisting of
    // asset name, font family, weight, stretch, style, remove button
    [NonSerialized]
    private List<Rect> _fontsAssetsRects = null;
    // Drag and drop information.
    [NonSerialized]
    private DragInfo _dragInfo = new DragInfo();
    // Current scroll position inside the list of input fonts
    [NonSerialized]
    private Vector2 _fontsListScrollPos = new Vector2(0, 0);
}
