# Generic Pool Manager(제네릭 풀 매니저)
제네릭 풀 매니저는 Liam Ederzeel의 튜토리얼(https://liamederzeel.com/a-generic-object-pool-for-unity3d/) 을 기반으로 한 풀 매니저입니다.
이 풀 매니저는 클래스, 모노비해비어를 상속받은 클래스, 프리팹을 생성하는데 사용할 수 있습니다.

## 설치
이 저장소의 주소를 복사하고, Package Manager → Add package from git url을 선택하여 붙여넣기 하면 됩니다.

## 예제
일반 클래스를 풀링할 때
```csharp
using Pool;

public class Example : MonoBehaviour {
    const START_SIZE = 5;
    Pool<Enemy> pool;

    void Start() {
        pool = new Pool<Enemy>(new Factory<Enemy>(), START_SIZE);
    }

	void Spawn() {
		Enemy enemy = pool.Allocate();

		EventHandler handler = null;
		handler = (sender, e) => {
			pool.Release(enemy);
			enemy.Death -= handler;
		};

		enemy.Death += handler;
		enemy.gameObject.SetActive(true);
	}
} 
```

모노비해비어(유니티) 파생 클래스를 풀링할 때
```csharp
using Pool;

public class Example : MonoBehaviour {
    const START_SIZE = 5;
    Pool<Enemy> pool;

    void Start() {
        pool = new Pool<Enemy>(new MonoBehaviourFactory<Enemy>(), START_SIZE);
    }

	void Spawn() {
		Enemy enemy = pool.Allocate();

		EventHandler handler = null;
		handler = (sender, e) => {
			pool.Release(enemy);
			enemy.Death -= handler;
		};

		enemy.Death += handler;
		enemy.gameObject.SetActive(true);
	}
} 
```

프리팹에 사용할 때
```csharp
using Pool;

public class Example : MonoBehaviour {
    const START_SIZE = 5;
    Pool<Enemy> pool;
    GameObject prefab;

    void Start() {
        pool = new Pool<Enemy>(new PrefabFactory<Enemy>(prefab), START_SIZE);
    }

	void Spawn() {
		Enemy enemy = pool.Allocate();

		EventHandler handler = null;
		
		// 생성시 사망 처리용 Death 이벤트핸들러를 직접 정의 (Lambda식 구현)
		handler = (sender, e) => {
			pool.Release(enemy);
			enemy.Death -= handler;
		};

		enemy.Death += handler;
		enemy.gameObject.SetActive(true);
	}
} 
```

풀에 넣을 프리팹을 설정할 때
```csharp
using Pool;
using System;

public class Enemy : MonoBehaviour, IResettable
{
	// Spawn에서 지정해 줄 이벤트핸들러
	public event EventHandler Death;
	
	public void Reset()
	{
		// 다시 풀로 올라갈 때 내용 구현 ex) 충돌체 리셋, HP 리셋 등등
		// IResettable 인터페이스의 기본형으로 반드시 구현해야 오류가 없음.
	}
}
```

용도에 맞는 새 팩토리를 만들어 사용할 때
```csharp
using Pool;

public class ExampleFactory<T> : IFactory<T> where T : new() {

	string foo;

	public ExampleFactory() : this("foo") { }
	public ExampleFactory(string foo) {
		this.foo = foo;
	}

	public T Create() {
		T bar = new T(name);
		return object;
	}
}
```
