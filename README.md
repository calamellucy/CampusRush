# Campus Rush v1.0.0

### 프로젝트 설명

지각 위기에 처한 대학생이 지각하지 않기 위해 달리는 러너 게임

Unity 2D 기반 팀프로젝트

---

### 주요 기능

- 플레이어 숙이기 및 점프
- 장애물 등장 및 충돌 처리
- 플레이어 HP 시스템
- 아이템 등장 및 획득
- 아이템 획득 시 점수 증가
- 배경 및 플레이어 에셋 적용
- 게임오버 처리

---

### 필요한 소프트웨어 및 하드웨어 요구 사항

- Unity Editor: Unity 6000.3.15f1
- Git
- GitHub Desktop
- Windows 또는 macOS

- Unity 2D 프로젝트 실행이 가능한 PC
- 최소 4GB RAM 이상 권장
- 키보드 입력 가능 환경

---

### 설치 및 설정 방법

1. GitHub 저장소를 클론

```
git clone 저장소_URL
```

2. 프로젝트에 맞는 Unity 버전으로 프로젝트 열기

3. Assets/Scenes 폴더에서 StartScene을 열고 실행

---

### 조작 방법과 게임 규칙

#### 조작 방법

- SPACE: 점프
- SPACE x2: 이단 점프
- 아래쪽 방향키: 숙이기

#### 게임 규칙

- Player가 장애물에 닿으면 HP 1 감소
- HP가 0이 되면 게임 오버
- Item 획득 시 점수 100 증가

---

### 기여 방법

브랜치 전략: Git Flow

- main: 최종 배포 브랜치
- develop: 개발 통합 브랜치
- feature/\*: 기능 개발 브랜치
- release-\*: 릴리즈 준비 및 QA 브랜치
- fix/\*: 버그 수정 브랜치

작업 방식

- Issue 탭에 이슈 등록
- develop 브랜치에서 feature 브랜치 생성
- 기능 구현 후 Pull Request 생성
- 코드 확인 및 테스트 후 develop 브랜치에 병합
- 병합 후 해당 feature 브랜치 삭제

---

### 저자 및 연락처 정보

세종대학교 오픈소스SW개론 2분반 팀프로젝트 4조

김수아 <https://github.com/calamellucy>

구가영 <https://github.com/9ga0>

김예린 <https://github.com/yurum2>

송채원 <https://github.com/thdcothd>

---
