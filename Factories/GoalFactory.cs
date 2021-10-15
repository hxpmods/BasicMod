using BasicMod.GameHooks;
using Books.GoalsBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Bookmarks.BookmarkControllersGroupController;

namespace BasicMod.Factories
{
    public static class GoalFactory
    {
        public static List<Goal> allGoals = new List<Goal>();
        public static List<ChaptersGroup> allChaptersGroups = new List<ChaptersGroup>();

        public static void Awake()
        {


            /*
            GoalManagerStartEvent.OnGoalManagerStart += (sender, e) =>
            {               
                RegisterChapters();
                LogChapters();
            };*/

            GameStartEvent.OnGameStart += (sender, e) =>
            {
                RegisterChapters();
                //LogChapters();
            };

            GoalsLoaderGetByName.GoalsLoaderGetByNameEvent += (sender, e) =>
            {
                Goal findGoal = (from goal in allGoals
                                 where goal.name == e.name
                                 select goal).FirstOrDefault();

                if (findGoal!= null)
                {
                    e.handled = true;
                    e.result = findGoal;
                }

            };


        }

        public static Goal CreateGoal(string name)
        {
            Goal goal = ScriptableObject.CreateInstance<Goal>();
            goal.name = name;
            AddGoal(goal);
            return goal;
        }

        public static Chapter CreateChapter(string name)
        {
            Chapter chapter = new Chapter();
            chapter.name = name;
            chapter.goals = new List<Goal>();

            return chapter;
        }

        public static ChaptersGroup CreateChaptersGroup(string name)
        {
            ChaptersGroup chapterg = new ChaptersGroup();
            chapterg.name = name;
            allChaptersGroups.Add(chapterg);
            return chapterg;
        }

        public static void AddGoal(Goal goal)
        {
            allGoals.Add(goal);
        }

        public static void AddGoalToChapter(Goal goal, Chapter chapter)
        {
            chapter.goals.Add(goal);
        }

        public static void AddChapterToChapterGroup(Chapter chapter, ChaptersGroup chaptersg)
        {
            chaptersg.chapters.Add(chapter);
        }

        public static void RegisterChapters() {
            //int chapterGroupsAdded = 0;
            foreach (ChaptersGroup chaptersg in allChaptersGroups)
            {
                Managers.Goals.settings.chaptersGroups.Add(chaptersg);
                CreateBookmarkControllerForChaptersGroup(chaptersg);
               
                //chapterGroupsAdded++;
            }
        }

        private static void CreateBookmarkControllerForChaptersGroup(ChaptersGroup chaptersg)
        {
            Bookmarks.BookmarkController controller = UnityEngine.Object.Instantiate(Managers.Goals.goalsBook.bookmarkControllersGroupController.controllers[0].bookmarkController, Managers.Goals.goalsBook.bookmarkControllersGroupController.controllers[0].bookmarkController.transform);
            controller.gameObject.transform.position = controller.gameObject.transform.position + Vector3.right * 8;

            var bookmark = new BookmarkControllerWithTitle();
            bookmark.name = chaptersg.name;
            bookmark.bookmarkController = controller;
            Managers.Goals.goalsBook.bookmarkControllersGroupController.controllers.Add(bookmark);
        }

        public static void LogChapters()
        {
            Debug.Log("Beginning chapter Log");
            foreach (ChaptersGroup chapters in Managers.Goals.settings.chaptersGroups)
            {
                Debug.Log("Group:\n");
                Debug.Log(chapters.name);

                foreach(Chapter chapter in chapters.chapters)
                {
                    Debug.Log("Chapter:\n");
                    Debug.Log(chapter.name);
                    Debug.Log("Goals:\n");
                    foreach (Goal goal in chapter.goals)
                    {
                        Debug.Log(goal.name);
                    }

                }

            }
        }

    }
}
