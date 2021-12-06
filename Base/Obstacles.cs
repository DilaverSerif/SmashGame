using UnityEngine;

public class Obstacles : Collectable
{
    private bool oneTime;
    
    protected override bool condition(GameObject contant)
    {
        if (contant.GetComponent<Player>() != null & !oneTime)
        {
            oneTime = true;
            return true;
        }

        return false;
    }

    public override void Contact(GameObject target)
    {
        GameManager.Instance.PizzaSpawn(-value);

        if (GameManager.Instance.pizzaList.Count <= 0 & value > 0)
        {
            GameBase.FailGame.Invoke();
        }
    }
    
    public override void Effect(Vector3 pos = new Vector3())
    {
        GameBase.Dilaver.SoundSystem.PlaySound(sound);
        
        GameBase.Dilaver.ParticlePlaySystem.SetScale(Vector3.one * 5).
            PlayParticle(_particle,target.transform.position);

        if (canDestory)
        {
            Destroy(gameObject);
        }
    }
}