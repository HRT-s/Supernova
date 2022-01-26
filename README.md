# 五連星並べ

## 概要
五目並べをモチーフとしたボードゲーム

プレイヤーは交互に星を配置し、先に5つ縦、横、斜めのいずれかで揃えた方の勝利

オンライン対戦とローカル対戦が可能

## 使用言語・ツール等
![C#](https://img.shields.io/badge/c%23%20-%23239120.svg?&style=for-the-badge&logo=c-sharp&logoColor=white)
![Unity](https://img.shields.io/badge/unity%20-%23000000.svg?&style=for-the-badge&logo=unity&logoColor=white)

## ルール
プレイヤーは1ターン中に2回行動をすることができる。
- 星の設置...盤面に星を置く
- 星の回収...盤面の星を取り除く
- 小爆発...盤面の自分の星を爆発させ、周囲の星を取り除く

「星の回収」「小爆発」を使用すると、クールタイムが発生する。

先攻は2ターン、後攻は1ターン使用不可となる。

## その他
オンライン対戦において、先にマッチングを開始したプレイヤーが先攻となる。

ネットワークの実装にはPhoton Unity Networking 2を使用。

## 素材提供
チコデザ

ニコニ・コモンズ

CoolText