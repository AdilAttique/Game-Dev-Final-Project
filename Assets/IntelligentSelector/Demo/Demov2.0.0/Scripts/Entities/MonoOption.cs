using System;
using System.Collections;
using UnityEngine;

namespace Assets.IntelligentSelector.Demo.Demov2._0._0.Scripts.Entities
{
    [Serializable]
    public class MonoOption : MonoBehaviour
    {
        [SerializeField]
        Material _matOnSelect;

        [SerializeField]
        Material _matOnDeSelect;

        [SerializeField]
        Material _matOnConsider;

        float _scaleParameter = 1.25f;
        float _scaleSpeed = 0.85f;
        Vector3 _initScale;


        void Awake()
        {
            _initScale = transform.localScale;
        }

        /// <summary>
        /// OnDeSelect method
        /// </summary>
        public void OnDeSelect()
        {
            //set the material
            gameObject.GetComponent<Renderer>().material = _matOnDeSelect;

            //scale me in
            StartCoroutine(ScaleIn());
        }

        /// <summary>
        /// OnConsider method
        /// </summary>
        public void OnConsider()
        {
            //set the material
            gameObject.GetComponent<Renderer>().material = _matOnConsider;
        }

        /// <summary>
        /// OnSelect method
        /// </summary>
        public void OnSelect()
        {
            //set the material
            gameObject.GetComponent<Renderer>().material = _matOnSelect;

            //scale me out
            StartCoroutine(ScaleOut());
        }

        /// <summary>
        /// OnUnConsider method
        /// </summary>
        public void OnUnConsider()
        {
            //set the material
            gameObject.GetComponent<Renderer>().material = _matOnDeSelect;
        }

        /// <summary>
        /// Scales this instance out
        /// </summary>
        /// <returns></returns>
        private IEnumerator ScaleIn()
        {
            //find the resultant to scale to
            Vector3 resultantScale = _initScale;
           
            while(resultantScale.magnitude != transform.localScale.magnitude)
            {
                //scale in
                transform.localScale = Vector3.MoveTowards(transform.localScale,
                    resultantScale,
                    _scaleSpeed * Time.deltaTime);

                //wait for the time
                yield return new WaitForSeconds(0f);
            }
        }

        /// <summary>
        /// Scales this instance in
        /// </summary>
        /// <returns></returns>
        private IEnumerator ScaleOut()
        {
            //find the resultant to scale to
            Vector3 resultantScale = transform.localScale * _scaleParameter;

            while (resultantScale.magnitude != transform.localScale.magnitude)
            {
                //scale in
                transform.localScale = Vector3.MoveTowards(transform.localScale,
                    resultantScale,
                    _scaleSpeed * Time.deltaTime);

                //wait for the time
                yield return new WaitForSeconds(0f);
            }
        }
    }
}
