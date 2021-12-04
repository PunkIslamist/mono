(def example-values [199 200 208 210 200 207 240 269 260 263])

(defn pairs [values]
  (loop [left values right (next left) current []]
    (let [[lhead & ltail] left [rhead & rtail] right]
      (if
        (not rhead) current
        (recur ltail rtail (conj current [lhead rhead]))))))

(defn increasing [values]
  (let [firstLessThanSecond #(< (first %) (second %))]
  (count (filter firstLessThanSecond (pairs values)))))

(println "Example count:" (increasing example-values))