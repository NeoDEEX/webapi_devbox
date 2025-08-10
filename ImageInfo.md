# About **neodeex.webapi** Docker Image

neodeex.webapi 도커 이미지는 Fox Biz/Data Service Web API 를 빠르게 구성하고 테스트하기 위해 미리 구축된 ASP.NET Core Web API 앱을 제공합니다. 개발자는 이 이미지를 사용하여 비즈 모듈이나 Fox Query를 빠르게 구성하고 테스트할 수 있습니다.

## Docker image overview

Ubuntu 환경에서 ASP.NET Core 8.0을 사용하며 `/app` 디렉터리에서 앱이 작동합니다.

* Ubuntu + ASP.NET Core 8.0

  필요에 따라 Ubuntu 버전과 ASP.NET Core 버전은 변경될 수 있습니다.

* NeoDEEX 5.x

  최신 NeoDEEX 5.x 버전이 적용되며 아직 Release 되지 않은 미리 보기(preview) 빌드가 적용될 수도 있습니다.

### Connection String Configuration

이미지에서 제공되는 기본 데이터베이스 연결은 PostgreSQL 데이터 베이스이며 Foxml 설정은 `/app/foxml` 폴더를 사용합니다. 데이터베이스 연결은 [구성 설정 커스터마이징](./README.md#configuration-customizing)을 통해 실제 사용하고자하는 데이터베이스 연결 문자열을 지정해야 합니다.

```json
  "database": {
    "connectionStrings": {
      "DefaultDB": {
        "connectionString": "your connection string",
        "type": "NeoDEEX.Data.NpgsqlClient.FoxNpgsqlDbAccess",
        "queryMapper": "DefaultMapper"
      }
    },
    "queryMappers": {
      "DefaultMapper": {
        "directories": [ "./foxml" ]
      }
    }
  },
```

### Fox Biz/Data Service Configuration

이미지에서 기본적으로 구성된 Fox Biz/Data Service 설정은 다음과 같습니다. 비즈 모듈은 `/app/bizmodules` 폴더의 모든 `.dll` 파일을 사용하며 모듈 다시 로드 기능은 꺼져있습니다(도커 환경 상 사용하지 않음). 비즈/데이터 서비스 모두 진단 기능(`"diagnostics:enable"` 속성)이 활성화 되어 있으며, 데이터 서비스의 경우 클라이언트가 요청한 경우 상세한 DB Profile 정보를 수집하고 반환(`detailedDbProfile` 속성)합니다.

```json
  "bizService": {
    "modules": [
      "./bizmodules/*.dll"
    ],
    "diagnostics": {
      "enable": true,
      "loggerName": "bizservice"
    },
    "useLoadContext": false
  },
  "dataService": {
    "diagnostics": {
      "enable": true,
      "detailedDbProfile": true,
      "loggerName": "dataservice"
    }
  },
```

### Web API Configuration

기본적으로 구성된 Web API 설정은 다음과 같아서 타입 정보를 사용하지 않는 JSON 결과를 반환(`useTypeInfo` 속성)하며 상세한 오류 정보(`detailedErrorInfo` 속성) 및 서버 정보(`returnServerInfo` 속성)를 반환합니다.

```json
  "webapiServer": {
    "useTypeInfo": false,
    "detailedErrorInfo": true,
    "returnServerInfo": true
  },
```

### Logging Configuration

기본적으로 모든 로깅은 콘솔에 표시되며 도커의 기능을 통해 모니터링 및 확인이 가능합니다.

```json
  "logging": {
    "filter": "Verbose",
    "loggers": {
      "dataservice": {
        "providerType": "NeoDEEX.Diagnostics.Loggers.FoxConsoleLoggerProvider"
      },
      "bizservice": {
        "providerType": "NeoDEEX.Diagnostics.Loggers.FoxConsoleLoggerProvider"
      }
    }
  }
```

---
