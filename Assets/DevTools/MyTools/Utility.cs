using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Client.DevTools.MyTools
{
    public static class Utility
    {
        public static Color enabledColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public static Color disabledColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        public static Color halfAlpha = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        public static Color exploredAlpha = new Color(1.0f, 1.0f, 1.0f, 0.75f); // 0.6f
        public static Color zeroAlpha = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        private static string SaveDataPathName = "SaveData"; // readonly!
        public static readonly float[] IslandAngle = { 45.0f, 135.0f, 225.0f, 315.0f };
        public static readonly float[] RandomTurnIslandAngle = { 0.0f, 90.0f, 180.0f, 270.0f };
        public static Vector3 MousePointInWorld()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return new Vector3(ray.origin.x, ray.origin.y, 0);
        }

        public static int GetNumberFromString(string str)
        {
            string resultString = System.Text.RegularExpressions.Regex.Match(str, @"\d+").Value;
            return System.Int32.Parse(resultString);
        }
        
        public static float GetRandomIslandAngle() => IslandAngle[Random.Range(0, IslandAngle.Length)];
        public static float GetRandomTurnIslandAngle() => RandomTurnIslandAngle[Random.Range(0, RandomTurnIslandAngle.Length)];

        public static string ConvertToTime(float seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds + 1);
            var h = t.TotalHours > 1 ? ((int)t.TotalHours).ToString() + ":" : "";
            return h + string.Format("{1:D2}:{2:D2}", (int)t.TotalHours, t.Minutes, t.Seconds);
        }

        public static double Truncate2(double value, int digits)
        {
            double mult = Math.Pow(10.0, digits);
            double result = Math.Truncate(mult * value) / mult;
            return (float)result;
        }

        public static double RoundToX(double num, int digits = 2)
        {
            var c = (int)Math.Floor(Math.Log10(num) + 1);
            var res = num;
            if (digits < c)
            {
                double d = Mathf.Pow(10, c - digits);
                res = Math.Round(res / d) * d;
            }

            return res;
        }

        public static float GetHalfScreenWidth() => Screen.width / 2;

        static CultureInfo ci = new CultureInfo("en-us");

        public static string[] shortNotation = new string[23]
        {
            "", "k", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc", "U", "D", "a", "j", "aa", "jj", "!", "!!",
            "ss", "dd", "nn",
        };

        public static int ToLayer(LayerMask layerMask) => (int)Mathf.Log(layerMask.value, 2);

        public static string FormatMoney(double target, string lowDecimalFormat = "N0", int maxValue = 1000,
            int minValue = 1000, bool isIgnoreLowDecWhenLessThanMinValue = false, bool auto = true)
        {
            double value = target;
            int baseValue = 0;
            string notationValue = "";
            string toStringValue;

            if (value >= maxValue)
            {
                value /= 1000;
                baseValue++;
                while (Mathf.Round((float)value) >= minValue)
                {
                    value /= 1000;
                    baseValue++;
                }

                string[] parts = value.ToString().Split('.');
                double part1 = double.Parse(parts[0]);
                if (auto)
                {
                    if (part1.ToString().Length == 3)
                        toStringValue = "N0";
                    else if (part1.ToString().Length == 2)
                        toStringValue = "N1";
                    else if (part1.ToString().Length == 4)
                        toStringValue = "N0";
                    else if (part1.ToString().Length == 5)
                        toStringValue = "N0";
                    else
                        toStringValue = "N2";
                }
                else
                    toStringValue = lowDecimalFormat;

                if (baseValue > shortNotation.Length) return null;
                else notationValue = shortNotation[baseValue];
                return value.ToString(toStringValue) + notationValue;
            }
            else if (isIgnoreLowDecWhenLessThanMinValue)
            {
                if (target < 0d && target > -minValue)
                    toStringValue = "N0";
                else if (target > 0d && target < minValue)
                    toStringValue = "N0";
                else
                    toStringValue = lowDecimalFormat;
            }
            else
                toStringValue = lowDecimalFormat; // string formatting at low numbers

            return value.ToString(toStringValue) + notationValue;
        }

        public static long Fib(int n)
        {
            long a = 0;
            long b = 1;
            // In N steps compute Fibonacci sequence iteratively.
            for (int i = 0; i < n; i++)
            {
                long temp = a;
                a = b;
                b = temp + b;
            }

            return a;
        }

        public static long Fac(long x)
        {
            return (x == 0) ? 1 : x * Fac(x - 1);
        }

        public static List<T> GetShuffleList<T>(List<T> ts)
        {
            List<T> list = new List<T>(ts);

            var count = list.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = Random.Range(i, count);
                (list[i], list[r]) = (list[r], list[i]);
            }

            return list;
        }

        public static class Animation
        {
            public static void ResetAnimator(Animator animator)
            {
                animator.Rebind();
                animator.Update(0f);
                //animator.Play("Idle", 0, 0f);
            }

            public static void SetAnimation(Animator animator, int animation, int randomAnimation = -1,
                string animationRandom = null)
            {
                if (animator.GetBool(animation))
                    return;

                //ResetAnimator(animator);

                if (animationRandom != null)
                    animator.SetInteger(animationRandom, randomAnimation);

                animator.SetTrigger(animation);
            }

            public static void SetAnimationSpeed(Animator animator, string multiplierName, float speed) =>
                animator.SetFloat(multiplierName, speed);

            public static void SetAnimationBool(Animator animator, string stateName, bool state) =>
                animator.SetBool(stateName, state);

            public static void SetAnimationNumber(Animator animator, string stateName, int num) =>
                animator.SetInteger(stateName, num);

            public static void ResetAnimationTrigger(Animator animator, int animation) =>
                animator.ResetTrigger(animation);
        }

        public static void ResetRigibodyVelocity(Rigidbody _rigidbody)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public static bool ApproximatelyQuaternions(Quaternion quatA, Quaternion value, float acceptableRange)
        {
            return 1 - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
        }

        public static string GetDataPath()
        {
            return Path.Combine(Application.persistentDataPath, SaveDataPathName);
        }
        
#if UNITY_EDITOR
        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name); //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++) //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }
#endif

        public static void ScrollToTop(ScrollRect scrollRect)
        {
            scrollRect.DOVerticalNormalizedPos(1f, 0.1f);
        }
        
        public static bool IsPointerOverUIObject()
        {
            if (EventSystem.current == null) return false;
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }

    public static class Names
    {
        public static string[] names =
        {
            "Allison", "Arthur", "Ana", "Alex", "Arlene", "Alberto", "Aki", "Ayumi", "Akane",
            "Barry", "Bertha", "Bill", "Bonnie", "Bret", "Beryl",
            "Chantal", "Cristobal", "Claudette", "Charley", "Cindy", "Chris", "Chiaki",
            "Dean", "Dolly", "Danny", "Danielle", "Dennis", "Debby",
            "Erin", "Edouard", "Erika", "Earl", "Emily", "Ernesto", "Emi", "Etsuko",
            "Felix", "Fay", "Fabian", "Frances", "Franklin", "Florence",
            "Gabielle", "Gustav", "Grace", "Gaston", "Gert", "Gordon",
            "Humberto", "Hanna", "Henri", "Hermine", "Harvey", "Helene", "Hitomi",
            "Iris", "Isidore", "Isabel", "Ivan", "Irene", "Isaac", "Itoe",
            "Jerry", "Josephine", "Juan", "Jeanne", "Jose", "Joyce", "Junko",
            "Karen", "Kyle", "Kate", "Karl", "Katrina", "Kirk", "Kumiko", "Kaori", "Kazuko",
            "Lorenzo", "Lili", "Larry", "Lisa", "Lee", "Leslie",
            "Michelle", "Marco", "Mindy", "Maria", "Michael", "Miharu", "Michiyo", "Miyuki", "Miwa", "Miyako", "Mieko",
            "Noel", "Nana", "Nicholas", "Nicole", "Nate", "Nadine", "Nanaho", "Naoko",
            "Olga", "Omar", "Odette", "Otto", "Ophelia", "Oscar",
            "Pablo", "Paloma", "Peter", "Paula", "Philippe", "Patty",
            "Rebekah", "Rene", "Rose", "Richard", "Rita", "Rafael", "Reina", "Rie",
            "Sebastien", "Sally", "Sam", "Shary", "Stan", "Sandy", "Sayuri", "Sachiko",
            "Tanya", "Teddy", "Teresa", "Tomas", "Tammy", "Tony", "Toshie",
            "Van", "Vicky", "Victor", "Virginie", "Vince", "Valerie",
            "Yumi", "Youko",
            "Wendy", "Wilfred", "Wanda", "Walter", "Wilma", "William"
        };

        public static string GetRandom()
        {
            return names[Random.Range(0, names.Length)];
        }
    }
}