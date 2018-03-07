# スケジュール
3/10 ゲーム部分完成
3/17 UIもろもろ完成
3/24 リリース準備完了

# やること

### ゲーム

 1. 餌をたべる
 2. 餌を食べないと時間で死ぬ
 3. 餌を食べると生存時間が増える
 4. 餌を放置すると成長する（小→大→腐→ゾンビ）
 5. ゾンビは自分を追いかけてくる（3種類くらいAI作成）
 6. 

### UI

 1. 胃袋
 2. スコア
 3. タイトル

### その他

 1. ランキング
 2. AdMob

## UML 表示テスト


```mermaid
sequenceDiagram
Alice ->> Bob: Hello Bob, how are you?
Bob-->>John: How about you John?
Bob--x Alice: I am good thanks!
Bob-x John: I am good thanks!
Note right of John: Bob thinks a long<br/>long time, so long<br/>that the text does<br/>not fit on a row.

Bob-->Alice: Checking with John...
Alice->John: Yes... John, how are you?
```


```mermaid
graph LR
A[Square Rect] -- Link text --> B((Circle))
A --> C(Round Rect)
B --> D{Rhombus}
C --> D
```