using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
	public BulletPatternList[] bulletPatterns;
	public float[] weights;
	public float interval;

	public bool isSpawningBullets;

	void Start()
	{
		StartPattern();
		gameObject.AddComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		UpdatePatternPos();
	}

	void StartPattern()
    {
        SetAllPatternsRun(true);
        StartCoroutine(WaitAndRunBullets());
    }

	IEnumerator WaitAndRunBullets()
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

				StartCoroutine(pat.InstantiateBullets(this.gameObject));
            }
			yield return new WaitForSeconds(maxWait);
		}
		yield return null;
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
		for (int i = 0; i < weights.Length; i++) sum += weights[i];
		int index = 0;
		while(index < weights.Length-1)
        {
			if (Random.Range(0, sum) < weights[index])
				return index;
			sum -= weights[index++];
        }
		return index;
    }
}

