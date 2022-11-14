using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Linq;

public class BulletSpawner : MonoBehaviour
{
	public BulletPatternList[] bulletPatterns;
	public float[] weights;
	public float interval;
    public bool spawnOnStart;

	public bool isSpawningBullets;

    public int difficulty = 0;

    protected float[] bulletAmountDifficultyMod = { 1, 1.2f };
    protected float[] bulletSpeedDifficultyMod = { 1, 1.3f };
    protected float[] durationDifficultyMod = { 1, 1.3f };

    public BulletPatternList[] addedDifficultyPatterns1;
    public BulletPatternList[] addedDifficultyPatterns2;

	void Start()
	{
        if(spawnOnStart)
		    StartPattern();
		gameObject.AddComponent<AudioSource>();
        
	}

    public void SetDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
        if (difficulty > 0)
        {
            bulletPatterns = bulletPatterns.Concat(addedDifficultyPatterns1).ToArray();

        }
        if (difficulty > 1)
        {
            bulletPatterns = bulletPatterns.Concat(addedDifficultyPatterns2).ToArray();
        }
    }

	// Update is called once per frame
	void Update()
	{
		UpdatePatternPos();
	}

	void StartPattern()
    {
        SetAllPatternsRun(true);
        StartCoroutine(WaitAndRunBulletsPermenant());
    }

    public void RunPatternOnce()
    {
        SetAllPatternsRun(true);
        StartCoroutine(SpawnBulletsOnce());
    }

	IEnumerator WaitAndRunBulletsPermenant()
	{
		while (isSpawningBullets)
		{
			yield return new WaitForSeconds(interval);
			int selection = SelectRandomPattern();
			float maxWait = bulletPatterns[selection].bulletHolder[0].duration;
			for(int i = 0; i < bulletPatterns[selection].Length(); i++)
            {
				maxWait = (bulletPatterns[selection].bulletHolder[i].duration > maxWait) ? bulletPatterns[selection].bulletHolder[i].duration : maxWait;
				BulletPattern pat = bulletPatterns[selection].bulletHolder[i];

				StartCoroutine(InstantiateBullets(pat));
            }
			yield return new WaitForSeconds(maxWait);
		}
		yield return null;
    }

    IEnumerator RunBulletsAndWait()
    {
        while (isSpawningBullets)
        {
            int selection = SelectRandomPattern();
            float maxWait = bulletPatterns[selection].bulletHolder[0].duration;
            for (int i = 0; i < bulletPatterns[selection].Length(); i++)
            {
                maxWait = (bulletPatterns[selection].bulletHolder[i].duration > maxWait) ? bulletPatterns[selection].bulletHolder[i].duration : maxWait;
                BulletPattern pat = bulletPatterns[selection].bulletHolder[i];

                StartCoroutine(InstantiateBullets(pat));
            }
            yield return new WaitForSeconds(maxWait+interval);
        }
        yield return null;
    }

    IEnumerator SpawnBulletsOnce()
    {
        int selection = SelectRandomPattern();
        float maxWait = bulletPatterns[selection].bulletHolder[0].duration;
        for (int i = 0; i < bulletPatterns[selection].Length(); i++)
        {
            maxWait = (bulletPatterns[selection].bulletHolder[i].duration > maxWait) ? bulletPatterns[selection].bulletHolder[i].duration : maxWait;
            BulletPattern pat = bulletPatterns[selection].bulletHolder[i];

            StartCoroutine(InstantiateBullets(pat));
        }
        yield return new WaitForSeconds(maxWait + interval);
    }

	public void SetAllPatternsRun(bool statusUpdate)
    {
		for (int i = 0; i < bulletPatterns.Length; i++)
			for (int j = 0; j < bulletPatterns[i].Length(); j++)
				bulletPatterns[i].bulletHolder[j].isRunning = statusUpdate;
    }

	public void UpdatePatternPos()
    {
        for (int i = 0; i < bulletPatterns.Length; i++)
            for (int j = 0; j < bulletPatterns[i].Length(); j++)
                bulletPatterns[i].bulletHolder[j].myPosition = transform.position;
    }

	public int SelectRandomPattern()
    {
		float sum = 0;
		for (int i = 0; i < Mathf.Min(weights.Length,bulletPatterns.Length); i++) sum += weights[i];
		int index = 0;
		while(index < Mathf.Min(weights.Length, bulletPatterns.Length) - 1)
        {
			if (Random.Range(0, sum) < weights[index])
				return index;
			sum -= weights[index++];
        }
		return index;
    }

    public virtual IEnumerator InstantiateBullets(BulletPattern pat)
    {
        Vector3 direction;
        float angle = pat.angle;
        //to implement later
        if (pat.shootAtPlayer)
        {
            direction = -transform.position + GetNearestPlayer();
            angle = Mathf.Asin(direction.y / direction.magnitude) * Mathf.Rad2Deg;
            if (direction.x < 0)
                angle = 180 - angle;
        }
        else if(pat.motionDir!=0)
        {
            direction = pat.motionDir * GetComponent<EnemyBehaviour>().velocityDir;
            if (direction.magnitude == 0)
                angle = pat.angle;
            else
                angle = Mathf.Asin(direction.y / direction.magnitude) * Mathf.Rad2Deg;
            if (direction.x < 0)
                angle = 180 - angle;
        }

        if (pat.randomAngle)
            angle = UnityEngine.Random.Range(0, 360);



        float rotationS = pat.rotationSpeed;
        float rotationA = pat.rotationAccel;
        float t = pat.duration * durationDifficultyMod[difficulty];
        float interv = pat.interval;

        float bS = pat.bulletS * bulletSpeedDifficultyMod[difficulty];
        float bA = pat.bulletA;
        float bJ = pat.bulletJ;

        int aOB = (int)(pat.amountOfBullets * bulletAmountDifficultyMod[difficulty]);
        int dAOB = pat.deltaAmountOfBullets;

        AudioSource audioSource = GetComponent<AudioSource>();
        float pitch = 1;

        while (isSpawningBullets && t > 0)
        {
            yield return new WaitForSeconds(interv);


            if (audioSource)
            {
                audioSource.clip = pat.mySound;
                audioSource.pitch = pitch;
                audioSource.volume = 0.125f;
                audioSource.Play();
            }

            for (int i = 1; i <= aOB; i++)
            {
                float ang = (angle - 0.5f * pat.rangeInDegrees + i * pat.rangeInDegrees / (float)aOB) * Mathf.Deg2Rad;
                Vector3 norm = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0);
                GameObject o = Instantiate(pat.bullet, transform.position, Quaternion.identity) as GameObject;
                Vector3 normA = Quaternion.AngleAxis(pat.bulletAAngle, Vector3.back) * norm;
                Vector3 normJ = Quaternion.AngleAxis(pat.bulletJAngle, Vector3.back) * norm;
                o.SendMessage("InitializeBullet", new BulletInitial(bS * norm, bA * normA, bJ * normJ));
            }

            bS += pat.deltaS * interv;
            bA += pat.deltaA * interv;
            bJ += pat.deltaJ * interv;

            angle += rotationS * interv;
            rotationS += rotationA * interv;
            rotationA += pat.rotationJerk * interv;

            pitch += pat.deltaPitch;

            if (aOB < 1)
                dAOB = -dAOB;
            aOB += dAOB;


            //to implement later
            if (pat.shootAtPlayer && pat.continueToShootAtPlayer)
            {
                direction = -transform.position + GetNearestPlayer();
                angle = Mathf.Asin(direction.y / direction.magnitude) * Mathf.Rad2Deg;
                if (direction.x < 0)
                    pat.angle = 180 - pat.angle;

            }
            else if (pat.motionDir != 0)
            {
                direction = pat.motionDir * GetComponent<EnemyBehaviour>().velocityDir;
                if (direction.magnitude == 0)
                    angle = pat.angle;
                else
                    angle = Mathf.Asin(direction.y / direction.magnitude) * Mathf.Rad2Deg;
                if (direction.x < 0)
                    angle = 180 - angle;
            }

            if (pat.overrideAngle)
            {
                pat.angle = angle;
            }

            interv = Mathf.Clamp(interv + pat.deltaInterval, 0.01f, Mathf.Infinity);
            t -= interv;
        }
        yield return null;
    }


    public Vector3 GetNearestPlayer()
    {
        Vector3 nearestPlayerDir = new Vector3(Screen.width, Screen.height, 0);
        foreach (GameObject o in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (o.CompareTag("Player"))
            {
                if ((o.transform.position - transform.position).magnitude < (nearestPlayerDir - transform.position).magnitude)
                {
                    nearestPlayerDir = o.transform.position;
                }
            }
        }
        return nearestPlayerDir;
    }
}

