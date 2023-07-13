# 배틀 오브 레전드 (공개용)

## Key Features
- 포톤 네트워크를 사용한 실시간 멀티플레이 (RPC)
- 플레이어, AI봇에 대한 행동패턴을 FSM(Finite-State Machine)으로 관리
- A* Star Algorithm을 통한 AI봇 이동경로 계산 (라이브러리 사용)
- 데이터와 GUI분리
- HTTP GET/POST 통신으로 서버로부터 유저/게임 데이터 조회 및 업데이트
- 골드/스태미나와 같은 유저 주요정보를 서버에서 관리 (firestore 사용)

## 필요패키지
- Firebase Unity SDK - FirebaseAuth
- Firebase Unity SDK - FirebaseAnalytics
- AstarPathfindingProject (https://assetstore.unity.com/packages/tools/behavior-ai/a-pathfinding-project-pro-87744)
- GoogleMobileAds
- Assets/StreamingAssets/ 에 google-services-desktop.json
- Assets/ 에 GoogleService-Info.plist


## 빌드확인
2023-7-11. Unity 2021.3.11f1 확인완료

