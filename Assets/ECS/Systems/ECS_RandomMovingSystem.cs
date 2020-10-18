using UnityEngine;
using System.Collections;
using FYFY;

namespace ECS
{
    public class ECS_RandomMoving_System : FSystem
    {
        private Family viruses = FamilyManager.getFamily(new AllOfComponents(typeof(ECS_RandomTarget)));

        // Use this to update member variables when system pause. 
        // Advice: avoid to update your families inside this function.
        protected override void onPause(int currentFrame)
        {

        }

        // Use this to update member variables when system resume.
        // Advice: avoid to update your families inside this function.
        protected override void onResume(int currentFrame)
        {

        }

        // Use to process your families.
        protected override void onProcess(int familiesUpdateCount)
        {
            foreach (GameObject go in viruses)
            {
                ECS_RandomTarget ecs_rt = go.GetComponent<ECS_RandomTarget>();
                if (go.transform.position == ecs_rt.target)
                {
                    ecs_rt.target = RandomTarget();
                }
            }
        }

        private Vector3 RandomTarget()
        {
            float x, y;

            x = Random.Range(-7f, 7f);
            y = Random.Range(-7f, 7f);

            return new Vector3(x, y, 0);
        }
    }
}