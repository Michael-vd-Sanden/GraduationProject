using UnityEditor;
using UnityEngine;
using ACInstrumentTools.Core;

namespace ACInstrumentTools.Core.Editor
{
    public class GuitarTunerWindow : EditorWindow
    {
        private FrequencyAnalyser frequencyAnalyser;
        private Analyse analyse = new Analyse();
        private bool showViewer = true;
        private bool showRawPeaks = false;
        private bool showCleanedPeaks = false;
        private bool onlyAnalyseInBoundPeaks = false;

        private GUIStyle padding = new GUIStyle();

        static Texture icon;

        [MenuItem("Window/Antoine Cherel/Guitar Tuner Tool")]
        public static void ShowEditorToolWindow()
        {
            GuitarTunerWindow wnd = GetWindow<GuitarTunerWindow>();
            wnd.titleContent = new GUIContent("Guitar Tuner Tool", icon);
        }

        private void CreateGUI()
        {
            icon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/AntoineCherel/InstrumentTools/Editor/frequencyIconSmall.png", typeof(Texture));
            padding.padding = new RectOffset(8, 8, 8, 8);
        }

        public void OnGUI()
        {
            FrequencyAnalyser analyser = FindObjectOfType<FrequencyAnalyser>();

            if (analyser != null && frequencyAnalyser == null)
            {
                frequencyAnalyser = analyser;
                frequencyAnalyser.onSampleAnalysed.AddListener(OnSampleAnalysed);
            }
            else if (analyser == null && frequencyAnalyser != null)
            {
                frequencyAnalyser.onSampleAnalysed.RemoveListener(OnSampleAnalysed);
                frequencyAnalyser = null;
            }
            else if (frequencyAnalyser != null && frequencyAnalyser != analyser)
            {
                frequencyAnalyser.onSampleAnalysed.RemoveListener(OnSampleAnalysed);
                frequencyAnalyser = analyser;
                frequencyAnalyser.onSampleAnalysed.AddListener(OnSampleAnalysed);
            }

            RenderGUI();
        }

        private void OnSampleAnalysed(Analyse a)
        {
            if (!onlyAnalyseInBoundPeaks || !a.fundamentalFrequencyOutOfBounds)
            {
                analyse = a;
                Repaint();
            }
        }

        private void RenderGUI()
        {
            GUILayout.BeginVertical(padding);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            Texture logo = (Texture)AssetDatabase.LoadAssetAtPath("Assets/AntoineCherel/InstrumentTools/Editor/frequencyAnalyserIcon.png", typeof(Texture));
            if (logo != null)
            {
                Rect rect = GUILayoutUtility.GetRect(32, 32);
                GUI.DrawTexture(rect, logo, ScaleMode.ScaleToFit);
            }
            GUILayout.Label("Guitar Tuner", EditorStyles.largeLabel);


            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(16);

            GUILayout.Label("Analyse Viewer", EditorStyles.largeLabel);


            GUILayout.Label("Settings", EditorStyles.centeredGreyMiniLabel);
            GUILayout.Space(8);

            showViewer = GUILayout.Toggle(showViewer, "Show Viewer");
            showRawPeaks = GUILayout.Toggle(showRawPeaks, "Show Raw Peaks");
            showCleanedPeaks = GUILayout.Toggle(showCleanedPeaks, "Show Cleaned Peaks");
            onlyAnalyseInBoundPeaks = GUILayout.Toggle(onlyAnalyseInBoundPeaks, "Only analyse 'in Bound' results");

            GUILayout.Space(16);
            GUILayout.Label("Viewer", EditorStyles.centeredGreyMiniLabel);
            GUILayout.Space(8);

            if (frequencyAnalyser == null)
            {
                GUILayout.Label("There is no FrequencyAnalyser in this Scene.");
                GUILayout.EndVertical();
                return;
            }

            if (showViewer)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Volume");
                GUILayout.FlexibleSpace();
                GUILayout.Label(analyse.sampleVolume.ToString("F1"), EditorStyles.boldLabel);
                GUILayout.EndHorizontal();
                GUILayout.Space(4);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Fundamental Frequency");
                GUILayout.FlexibleSpace();
                GUILayout.Label(analyse.fundamentalFrequency.ToString("F2") + " Hz", EditorStyles.boldLabel);
                GUILayout.EndHorizontal();
                GUILayout.Space(4);
                if (!onlyAnalyseInBoundPeaks)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Frequency Was Out of Bounds ");
                    GUILayout.FlexibleSpace();
                    GUILayout.Button(analyse.fundamentalFrequencyOutOfBounds.ToString());
                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);
                }
                if (showRawPeaks)
                {
                    GUILayout.Label("Raw Peaks");
                    RenderPeaks(analyse.peaks);
                    GUILayout.Space(4);
                }
                if (showCleanedPeaks)
                {
                    GUILayout.Label("Cleaned Peaks");

                    Peak[] cleanedPeaks = FrequencyUtils.CleanPeaks(analyse.peaks);
                    RenderPeaks(cleanedPeaks);
                    GUILayout.Space(4);
                }
            }
            GUILayout.EndVertical();
        }

        private bool ArePeaksNull(Peak[] peaks)
        {
            return (peaks == null || peaks.Length == 0);
        }

        private void RenderPeaks(Peak[] peaks)
        {
            if (ArePeaksNull(peaks))
                return;
                
            GUILayout.BeginVertical(padding);
            foreach (Peak peak in peaks)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(peak.frequency.ToString("F2") + " Hz");
                GUILayout.FlexibleSpace();
                GUILayout.Label(peak.value.ToString("F2") + " ");
                GUILayout.EndHorizontal();
                GUILayout.Space(4);
            }
            GUILayout.EndVertical();
        }

    }
}
