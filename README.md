# NeoDEEX Fox Biz/Data Service WebApi 개발 박스

도커 허브의 [neodeex.webapi](https://hub.docker.com/r/ksyu33/neodeex.webapi) 이미지를 사용하여 Fox Biz/Data Service Web API 개발 환경을 사용하는 예제 입니다.

## Usage

1. Database connection

    [`/config` 디렉터리의 `neodeex.config.json` 파일](./config/neodeex.config.json)을 편집하여 접속하고자 하는 데이터베이스 연결 문자열을 구성합니다.

    ```json
    {
      "$schema": "https://neodeex.github.io/doc/neodeex.config.schema.json",
      "$baseConfig": "neodeex.base.config.json",
      "database": {
        // SQL Server LocalDB 예제
        "connectionStrings": {
          "DefaultDB": {
            "connectionString": "Data Source=(LocalDB)\\MSSQLLocalDB;Database=Northwind;Integrated Security=True",
            "type": "NeoDEEX.Data.SqlClient.FoxSqlDbAccess"
          }
        }
      }
    }
    ```

2. Foxml 쿼리 설정

    [`/foxml` 디렉터리](/foxml/)에 사용하고자 하는 `.foxml` 파일(들)을 작성합니다.

3. Biz 로직 모듈 (옵션)

    비즈 로직 모듈 `.dll` (들)을 작성하고 빌드된 `.dll` 을 [`/bizmodules` 디렉터리](/bizmodules/)에 복사합니다.

4. Docker Compose 를 구동하여 컨테이너를 구동합니다.

    ```cmd
    docker compose up
    ```

5. Fox Data Service Web API 에 대한 테스트 페이지를 이용하여 Foxml 쿼리 테스트를 수행합니다.

    ```url
    http://localhost:5050/api/dataservice/test
    ```

6. Fox Biz Service Web API 에 대한 테스트 페이지를 이용하여 비즈 로직 호출 테스트를 수행합니다.

    ```url
    http://localhost:5050/api/bizservice/test
    ```

7. `.foxml` 파일 혹은 비즈 로직 모듈 `.dll` 파일이 변경된 경우 컨테이너를 재시작합니다.

    ```cmd
    docker compose restart
    ```

## Auto-Reload

비즈 모듈 `.dll` 이 변경되면 컨테이너를 자동으로 재시작 하도록 하거나, `.foxml` 파일의 변경을 감지하여 다시 로드하도록 설정이 가능합니다. 이 기능은 Docker Compose 의 Watch 기능을 사용합니다. 

> [!WARNING]
> Watch 기능이 정상적으로 작동하기 위해서는 `.dll` 파일 혹은 `.foxml` 파일의 날짜가 `neodeex.webapi` 이미지의 생성 날짜보다 더 최신이어야 합니다.

다음은 Compose Watch 기능을 활용하여 비즈 모듈과 `.foxml` 파일 변경을 확인하는 `docker-compose.yml` 파일의 부분을 보여줍니다.

> [!CAUTION]
> `foxml` 과 `bizmodules` 디렉터리에 대한 `mount` 설정을 제거하고 `watch` 설정을 추가하십시요.

```yml
    develop:
      watch:
        # Foxml 디렉터리 변경 사항을 컨테이너에 동기화
        - action: sync
          path: ./foxml
          target: /app/foxml
          x-initialSync: true
        # 비즈 모듈 디렉터리 변경 사항을 컨테이너에 동기화
        - action: sync+restart
          path: ./bizmodules
          target: /app/bizmodules
          x-initialSync: true
```

`neodeex.config.json` 파일 역시 Watch 에 의해 변경 시 컨테이너를 다시 시작할 수 있지만 도커가 이 파일의 변경을 인식하는 것과 재시작하는 것의 타이밍 상의 문제로 잘 적용이 안될 수 있습니다. 따라서 구성 설정 파일 변경 시 Watch 기능에 의존지 않고 명시적으로 컨테이너틀 다시 시작하는 것이 좋습니다.

Compose Watch 기능을 설정한 경우 --watch 옵션을 사용하여 Compose 를 구동하십시요.

```cmd
docker compose up --watch
```

---
