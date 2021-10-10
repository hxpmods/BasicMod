using Bookmarks;
using Books.RecipeBook;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using Utils;

namespace BasicMod
{
    class DuplicateRecipes
    {

        public static UnityEvents.BookmarkControllerInt onBookmarkCtrlClick = new UnityEvents.BookmarkControllerInt();
        public static void Awake()
        {
            onBookmarkCtrlClick.AddListener(OnBookmarkCtrlClick);
        }

        private static void OnBookmarkCtrlClick(BookmarkController arg0, int bookmarkid)
        {
           //Debug.Log(bookmarkid);
           RecipeBook book = Managers.Potion.recipeBook;
           Potion recipe = book.savedRecipes[bookmarkid];
           

        //Weird glitch where bookmarks don't untoggle. Don't really know how to fix it
           book.AddRecipe(recipe);
           //book.OpenPageAt(book.savedRecipes.Count+1);
        }
    }


    [HarmonyPatch(typeof(BookmarkControllersGroupController))]
    [HarmonyPatch("SetActiveBookmark")]
    class BookMarkClickedPatch
    {
        static bool Prefix( BookmarkControllersGroupController __instance, Bookmark bookmark, bool bookmarkClicked)
        {
            if (bookmarkClicked)
            {
                Debug.Log("Hi?");

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    int arg = __instance.GetAllBookmarksList().IndexOf(bookmark);
                    DuplicateRecipes.onBookmarkCtrlClick.Invoke(bookmark.rail.bookmarkController, arg);
                    //returning false auto selects new recipe but don't untick old one.
                    return true;
                }
            }

            return true;
        }
    }
}

