
using UnityEngine;
using System.Collections;

namespace NDR
{
    public class NDRUtility : MonoBehaviour
    {

        public static IEnumerator LerpLocalPosition(Transform agent, Vector3 targetPosition, float duration)
        {
            float time = 0;
            Vector3 startPosition = agent.localPosition;

            while (time < duration)
            {
                agent.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            agent.localPosition = targetPosition;
        }

        public static IEnumerator LerpVector3(Vector3 agent, Vector3 targetPosition, float duration)
        {
            float time = 0;
            Vector3 startPosition = agent;

            while (time < duration)
            {
                agent = Vector3.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            agent = targetPosition;
        }

        public static IEnumerator LerpLinear(float value, float targetValue, float duration)
        {
            float time = 0;
            float startingVlaue = value;

            while (time < duration)
            {
                value = Mathf.Lerp(startingVlaue, targetValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            value = targetValue;
        }


        public static IEnumerator LerpLocalEulerAngles(Transform agent, Vector3 targetRotation, float duration)
        {
            float time = 0;
            Vector3 startRotation = agent.localEulerAngles;

            while (time < duration)
            {
                agent.localEulerAngles = Vector3.Lerp(startRotation, targetRotation, time / duration);
                time += Time.deltaTime;

                yield return null;
            }

            agent.localEulerAngles = targetRotation;
        }

        public static IEnumerator LerpLocalEulerAngles(Transform agent, Vector3 targetRotation, float duration, Animator anim)
        {
            float time = 0;
            Vector3 startRotation = agent.localEulerAngles;
            anim.Play("CanvasGroupFadeOut");
            while (time < duration)
            {
                agent.localEulerAngles = Vector3.Lerp(startRotation, targetRotation, time / duration);
                time += Time.deltaTime;

                yield return null;
            }

            anim.Play("CanvasGroupFadeIn");

            agent.localEulerAngles = targetRotation;
        }

        public void OnCompleteLerp(Animator anim)
        {
            if(anim)
            {
                anim.Play("CanvasGroupFadeIn");
            }
        }
        public void OnStartLerp(Animator anim)
        {
            if (anim)
            {
                anim.Play("CanvasGroupFadeOut");
            }
        }


        
    }
}


namespace Masale_1
{
    using System;
    public class Class1
    {
        public static void Mohasebe()
        {
            int a = 0;
            int b = 0;

            int c;

            Console.WriteLine("Enter a");
            a = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter b");
            b = Convert.ToInt32(Console.ReadLine());

            c = a + b;

            Console.WriteLine("a + c = " + c);

            Console.ReadLine();
        }
    }
}

namespace Masale_2
{
    using System;
    public class Class2
    {
        public static void Mohasebe()
        {
            int a = 0;
            int b = 0;

            int c;

            Console.WriteLine("Enter a");
            a = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter b");
            b = Convert.ToInt32(Console.ReadLine());

            c = a + b;

            Console.WriteLine("a + c = " + c);

            Console.ReadLine();
        }
    }
}

namespace MehdiCSharpcar
{
    class Program
    {
        public static void Main(string[] arg)
        {
            Masale_1.Class1.Mohasebe();
            Masale_2.Class2.Mohasebe();
        }
    }
}


