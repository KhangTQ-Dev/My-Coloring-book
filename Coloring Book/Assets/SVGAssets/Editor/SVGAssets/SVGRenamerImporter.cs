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
using System.IO;
using UnityEditor;

public class SVGRenamerImporter : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets,
                                       string[] deletedAssets,
                                       string[] movedAssets,
                                       string[] movedFromAssetPaths)
    {
        foreach (string assetPath in importedAssets)
        {
            string fileExt = Path.GetExtension(assetPath);

            // SVG files
            if (fileExt.Equals(".svg", System.StringComparison.OrdinalIgnoreCase))
            {
                bool ok = true;
                // try to change ".svg" file extension with ".svg.txt", so Unity can recognize those files as text assets
                string newAssetPath = Path.ChangeExtension(assetPath, ".svg.txt");

                // does a file with this .svg.txt extension already exist?
                if (File.Exists(newAssetPath))
                {
                    ok = EditorUtility.DisplayDialog("SVG already exists!",
                                                     string.Format("{0} already exists, would you like to import it anyway and overwrite the previous one?", newAssetPath),
                                                     "Import and overwrite", "Do NOT import");
                    if (ok)
                    {
                        // remove the already existing asset file
                        AssetDatabase.DeleteAsset(newAssetPath);
                    }
                }
                // do the actual rename
                if (ok)
                {
                    // remove previous .meta (if exists)
                    FileUtil.DeleteFileOrDirectory(assetPath + ".meta");
                    // rename svg file by appending a .txt extension
                    FileUtil.MoveFileOrDirectory(assetPath, newAssetPath);
                    // refresh the database
                    AssetDatabase.Refresh();
                }
            }
            else
            // font files
            if (fileExt.Equals(".ttf", System.StringComparison.OrdinalIgnoreCase) ||
                fileExt.Equals(".otf", System.StringComparison.OrdinalIgnoreCase))
            {
                // because Unity already recognizes font files, we must warn the user
                bool ok = EditorUtility.DisplayDialog("Font import for SVGAssets",
                                                      string.Format("Would you like to import {0} font for SVGAssets? If no, Unity will import it as a TextMesh and it won't be usable from SVGAssets", assetPath),
                                                      "Import", "Do NOT import");
                if (ok)
                {
                    // try to change file extension by appending ".bytes", so Unity can recognize those files as (binary) text assets
                    string newAssetPath = Path.ChangeExtension(assetPath, fileExt.ToLower() + ".bytes");

                    // does a file with this .bytes extension already exist?
                    if (File.Exists(newAssetPath))
                    {
                        ok = EditorUtility.DisplayDialog("Font already exists!",
                                                         string.Format("{0} already exists, would you like to overwrite it?", newAssetPath),
                                                         "Import and overwrite", "Do NOT import");
                        if (ok)
                        {
                            // remove the already existing asset file
                            AssetDatabase.DeleteAsset(newAssetPath);
                        }
                    }

                    if (ok)
                    {
                        // remove previous .meta (if exists)
                        FileUtil.DeleteFileOrDirectory(assetPath + ".meta");
                        // rename font file by appending a .bytes extension
                        FileUtil.MoveFileOrDirectory(assetPath, newAssetPath);
                        // refresh the database
                        AssetDatabase.Refresh();
                    }
                }
            }
        }
    }
}
