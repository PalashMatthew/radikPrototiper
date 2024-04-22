// NULLcode Studio © 2015
// null-code.ru

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Sticky2D : MonoBehaviour {

	public float speed = 1.5f; // максимальная скорость
	public float acceleration = 100; // ускорение
	public float contactDistance = 1.25f; // дистанция с которой происходит сцепление с поверхностью
	public float gravityForce = 100; // гравитационная сила

	private int layerMask;
	private Rigidbody2D body;
	private float h;
	public Vector3 direction;
	public Vector3 gravity;

	public float inputSpeed;

	public bool isMoveAccess;

	public float timeToZamedlenie;
	bool isZamedlenie;
	float saveSpeed;

	public float flySpeed;


	void Awake()
	{
		isMoveAccess = true;

		body = GetComponent<Rigidbody2D>();
		body.gravityScale = 0;

		// нужно установить новый слой этому объекту, и назначить его, либо использовать стандартный Ignore Raycast
		// для триггеров и прочих объектов с которыми не должно быть взаимодействия

		// далее, выбранный слой будет исключен, включая стандартный слой Ignore Raycast
		layerMask = 1 << gameObject.layer | 1 << 2;
		layerMask = ~layerMask;

		// стартовое направление
		direction = Vector3.down;
		gravity = Vector3.down;

		GetDirections(direction, Mathf.Infinity);

		saveSpeed = speed;
	}

	void GetDirections(Vector3 currentDirection, float distance)
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, distance, layerMask);
		if(hit.collider)
		{
			SetNormal(hit.normal);
		}

		Scanner();
	}

	void SetNormal(Vector3 normal)
	{
		gravity = -normal.normalized; // вектор притяжения к плоскости
		direction = Vector3.Cross(normal, Vector3.forward).normalized; // вектор параллельно плоскости
	}

	void Scanner()
	{
		// пускаем лучи от центра во все стороны
		// заполняем массивы с векторами нормалей и дистанции до них
		int arr = 6;
		float j = 0;
		float[] distance = new float[arr];
		Vector2[] normal = new Vector2[arr];

		for (int i = 0; i < arr; i++)
		{
			var x = Mathf.Sin(j);
			var y = Mathf.Cos(j);
			
			j += 360 * Mathf.Deg2Rad / arr;
			
			Vector3 dir = transform.TransformDirection(new Vector3(x, y, 0));

			RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, layerMask);
			if(hit.collider)
			{
				distance[i] = hit.distance;
				normal[i] = hit.normal;
				Debug.DrawLine(transform.position, hit.point, Color.cyan);
			}
			else
			{
				distance[i] = Mathf.Infinity;
			}
		}

		float min = Mathf.Min(distance); // определяем наименьшую дистанцию до нормали

		// перебираем массив, если минимальная дистанция больше
		// чем дистанция прилипания (внешний угол) - меняем плоскость
		for (int i = 0; i < arr; i++)
		{
			if(distance[i] == min && min > contactDistance)
			{
				SetNormal(normal[i]);
			}
		}
	}

	void LateUpdate()
	{
		if (isMoveAccess)
		{
			//h = Input.GetAxis("Horizontal");
			h = inputSpeed;

			GetDirections(direction * h, contactDistance);

			Debug.DrawRay(transform.position, direction * h, Color.red);
		}
	}

	void FixedUpdate()
	{
		if (isMoveAccess)
		{
			body.AddForce(gravity * gravityForce * body.mass); // применяем гравитацию

			body.AddForce(direction * h * speed * acceleration * body.mass); // добавляем вектор движения

			// Ограничение скорости, иначе объект будет постоянно ускоряться
			if (Mathf.Abs(body.velocity.x) > speed)
			{
				body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * speed, body.velocity.y);
			}

			if (Mathf.Abs(body.velocity.y) > speed)
			{
				body.velocity = new Vector2(body.velocity.x, Mathf.Sign(body.velocity.y) * speed);
			}
		}
		else
		{
            body.AddForce(gravity * (gravityForce * flySpeed) * body.mass); // применяем гравитацию
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
		{
			isMoveAccess = false;
			direction = -direction;
			inputSpeed = -inputSpeed;
			//Vector3 vec = new Vector3(0, 1, 0);
			gravity = -gravity;
        }

		if (isZamedlenie)
		{
			speed += timeToZamedlenie;

			if (speed > saveSpeed)
			{
				speed = saveSpeed;
				isZamedlenie = false;
			}
		}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (!isMoveAccess)
		{
			isMoveAccess = true;
			//direction = -direction;
			isZamedlenie = true;
			speed = 0;
		}

		if (collision.tag == "finish" || collision.tag == "trap")
		{
			Application.LoadLevel(Application.loadedLevel);
		}
    }
}
