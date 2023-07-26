using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	public Shape[] allShapes;
	public GameObject[] nextBoard;
	public Transform[] queueXForms = new Transform[3];
	Shape[] queueShapes = new Shape[3];
	float queueScale = 0.3f;
	public Transform shapeTransform; 
	void Awake()
	{
		InitQueue();
	}
	void Start()
	{
		for (int i = 0; i < 3; i++)
		{
			Vector2 orgVector = new Vector2(nextBoard[i].transform.position.x, nextBoard[i].transform.position.y);
			Vector2 newVector = Vectorf.Round(orgVector);
		}
	}
	Shape GetRandomShape()
	{
		int i = Random.Range(0, allShapes.Length);
		Shape shape = allShapes[i];

		if (shape == null)
			return GetRandomShape();
		return shape;
	}
	public Shape SpawnShape()
	{
		Shape shape = null;
		shape = GetQueuedShape();
		shape.transform.position = transform.position;
		shape.transform.localScale = Vector3.one;
		shape.transform.parent = shapeTransform;
		if (shape)
			return shape;
		else
			return null;
	}
	void InitQueue()
	{
		for (int i = 0; i < queueShapes.Length; i++)
		{
			queueShapes[i] = null;
		}
		FillQueue();
	}
	void FillQueue()
	{
		for (int i = 0; i < queueShapes.Length; i++)
		{
			if (!queueShapes[i])
			{
				queueShapes[i] = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
				queueShapes[i].transform.position = queueXForms[i].transform.position + queueShapes[i].queueOffset;
				queueShapes[i].transform.localScale = new Vector3(queueScale, queueScale, queueScale);
				queueShapes[i].transform.parent = shapeTransform;
			}
		}
	}
	public Shape GetQueuedShape()
	{
		Shape firstShape = null;
		if (queueShapes[0])
			firstShape = queueShapes[0];
		for (int i = 1; i < queueShapes.Length; i++)
		{
			queueShapes[i - 1] = queueShapes[i];
			queueShapes[i - 1].transform.position = queueXForms[i - 1].position + queueShapes[i].queueOffset;
		}
		queueShapes[queueShapes.Length - 1] = null;
		FillQueue();
		return firstShape;
	}
}
