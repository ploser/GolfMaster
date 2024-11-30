using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GolfMaster
{
    public static class ExtensionsMonoBehaviour
    {
        public static Coroutine DelayedCall(this MonoBehaviour monoBehaviour, float delayTime, Action callback)
        {
            return monoBehaviour.StartCoroutine(DelayedCallProcess(delayTime, callback));
        }

        private static IEnumerator DelayedCallProcess(float delayTime, Action callback)
        {
            yield return new WaitForSeconds(delayTime);
            callback?.Invoke();
        }

        public static Coroutine RepeatEachFrame(this MonoBehaviour monoBehaviour, Action<float> callback)
        {
            return monoBehaviour.StartCoroutine(RepeatEachFrameProcess(callback));
        }

        public static IEnumerator RepeatEachFrameProcess(Action<float> callback)
        {
            while (true)
            {
                yield return null;
                callback?.Invoke(Time.deltaTime);
            }
        }

        public static Coroutine RepeatEachFrame(this MonoBehaviour monoBehaviour, Func<float, bool> callback)
        {
            return monoBehaviour.StartCoroutine(RepeatEachFrameProcess(callback));
        }

        public static IEnumerator RepeatEachFrameProcess(Func<float, bool> callback)
        {
            while (true)
            {
                if (callback == null || !callback.Invoke(Time.deltaTime))
                    break;

                yield return null;
            }
        }

        public static Coroutine RepeatCall(this MonoBehaviour monoBehaviour, float repeatTime, Action callback)
        {
            return monoBehaviour.StartCoroutine(RepeatCallProcess(repeatTime, callback));
        }

        private static IEnumerator RepeatCallProcess(float repeatTime, Action callback)
        {
            while (true)
            {
                yield return new WaitForSeconds(repeatTime);
                callback?.Invoke(); 
            }         
        }

        public static void StopCall(this MonoBehaviour monoBehaviour, Coroutine coroutine)
        {
            if (coroutine != null)
                monoBehaviour.StopCoroutine(coroutine);
        }

        public static WaiterObject WaitUntil(this MonoBehaviour monoBehaviour, Func<bool> waitingAction)
        {
            var wo = new WaiterObject();
            monoBehaviour.StartCoroutine(WaiterProcess(wo, waitingAction));
            return wo;
        }

        private static IEnumerator WaiterProcess(WaiterObject wo, Func<bool> caseFunc)
        {
            while (true)
            {
                yield return null;

                Debug.Log("check in waiter");
                if (caseFunc())
                    break;  
            }
            //yield return new WaitUntil(() => caseFunc());
            wo.Callback?.Invoke();
        }
    }

    public class WaiterObject
    {
        public Action Callback;
        public void Done(Action callback)
        {
            Callback = callback;
        }
    }
}
