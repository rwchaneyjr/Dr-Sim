using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace LearningLoop.ColoringBook
{
    public class ColoringPageCycler : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image coloringImage;

        [Header("Pages")]
        [Tooltip("Optional manual list. Leave empty to load p1 through p100 from Resources.")]
        [SerializeField] private Sprite[] pages;

        [Header("Auto Load From Resources")]
        [SerializeField] private bool autoLoadFromResources = true;
        [Tooltip("Folder under an Assets/Resources directory. Example: Assets/Resources/ColoringPages.")]
        [SerializeField] private string resourcesFolder = "ColoringPages";
        [SerializeField] private string pageNamePrefix = "p";
        [SerializeField] private int pageCount = 100;

        [Header("Optional Coloring System")]
        [Tooltip("Optional component with a RebuildTexture method. If blank, the scene is searched once.")]
        [SerializeField] private MonoBehaviour coloringSystem;
        [SerializeField] private string rebuildTextureMethodName = "RebuildTexture";

        private int currentPage;
        private MethodInfo rebuildTextureMethod;
        private bool searchedForColoringSystem;

        private void Start()
        {
            if (!EnsurePagesLoaded())
            {
                return;
            }

            ShowPage(0);
        }

        private bool EnsurePagesLoaded()
        {
            if (pages != null && pages.Length > 0)
            {
                return true;
            }

            if (!autoLoadFromResources)
            {
                Debug.LogError("No coloring pages assigned.", this);
                return false;
            }

            pages = LoadPagesFromResources();

            if (pages.Length > 0)
            {
                return true;
            }

            var lastPageName = $"{pageNamePrefix}{Mathf.Max(1, pageCount)}";
            Debug.LogError(
                $"No coloring pages found. Put sprites named {pageNamePrefix}1 through {lastPageName} in Assets/Resources/{resourcesFolder}.",
                this);
            return false;
        }

        private Sprite[] LoadPagesFromResources()
        {
            var totalPages = Mathf.Max(1, pageCount);
            var loadedPages = new List<Sprite>(totalPages);
            var missingPages = new List<string>();

            for (var pageNumber = 1; pageNumber <= totalPages; pageNumber++)
            {
                var resourcePath = $"{resourcesFolder}/{pageNamePrefix}{pageNumber}";
                var page = Resources.Load<Sprite>(resourcePath);

                if (page == null)
                {
                    missingPages.Add(resourcePath);
                    continue;
                }

                loadedPages.Add(page);
            }

            if (missingPages.Count > 0)
            {
                Debug.LogWarning(
                    $"Missing {missingPages.Count} coloring page(s) in Resources. First missing page: {missingPages[0]}",
                    this);
            }

            return loadedPages.ToArray();
        }

        private void ShowPage(int index)
        {
            if (coloringImage == null)
            {
                Debug.LogError("Coloring image is not assigned.", this);
                return;
            }

            if (pages == null || pages.Length == 0)
            {
                return;
            }

            currentPage = Mathf.Clamp(index, 0, pages.Length - 1);
            coloringImage.sprite = pages[currentPage];
            RebuildColoringTexture();
        }

        private void RebuildColoringTexture()
        {
            var target = GetColoringSystem();

            if (target == null || rebuildTextureMethod == null)
            {
                return;
            }

            rebuildTextureMethod.Invoke(target, null);
        }

        private MonoBehaviour GetColoringSystem()
        {
            if (coloringSystem != null)
            {
                rebuildTextureMethod ??= GetRebuildTextureMethod(coloringSystem);
                return coloringSystem;
            }

            if (searchedForColoringSystem)
            {
                return null;
            }

            searchedForColoringSystem = true;

            foreach (var behaviour in FindObjectsOfType<MonoBehaviour>())
            {
                var method = GetRebuildTextureMethod(behaviour);

                if (method == null)
                {
                    continue;
                }

                coloringSystem = behaviour;
                rebuildTextureMethod = method;
                return coloringSystem;
            }

            return null;
        }

        private MethodInfo GetRebuildTextureMethod(MonoBehaviour behaviour)
        {
            var method = behaviour.GetType().GetMethod(
                rebuildTextureMethodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (method == null || method.GetParameters().Length != 0)
            {
                return null;
            }

            return method;
        }

        public void NextPage()
        {
            if (pages == null || pages.Length == 0)
            {
                return;
            }

            ShowPage((currentPage + 1) % pages.Length);
        }

        public void PreviousPage()
        {
            if (pages == null || pages.Length == 0)
            {
                return;
            }

            ShowPage((currentPage - 1 + pages.Length) % pages.Length);
        }
    }
}
