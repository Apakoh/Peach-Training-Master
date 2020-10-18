using UnityEngine;
using FYFY;

namespace ECS
{
    public class ControllableSystem : FSystem
    {
        private Family _controllableGO = FamilyManager.getFamily(new AllOfComponents(typeof(ECS_Move)), new NoneOfComponents(typeof(ECS_RandomTarget)));
        private Family viruses = FamilyManager.getFamily(new AllOfComponents(typeof(ECS_Move), typeof(ECS_RandomTarget)));


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
            foreach (GameObject go in _controllableGO)
            {
                Vector3 movement = Vector3.zero;

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    movement += Vector3.left;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    movement += Vector3.right;
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    movement += Vector3.up;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    movement += Vector3.down;
                }

                go.transform.position += movement * go.GetComponent<ECS_Move>().speed * Time.deltaTime;
            }

            foreach (GameObject go in viruses)
            {
                MoveIA(go.GetComponent<ECS_RandomTarget>(), go.GetComponent<ECS_Move>());
            }
        }

        private void MoveIA(ECS_RandomTarget ecs_rt, ECS_Move ecs_m)
        {
            ecs_rt.transform.position = Vector3.MoveTowards(ecs_rt.transform.position, ecs_rt.target, ecs_m.speed * Time.deltaTime);
        }


    }
}