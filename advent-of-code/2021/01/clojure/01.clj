(require '[clojure.edn :as edn])
(require '[clojure.string :as string])

(def example-values [199 200 208 210 200 207 240 269 260 263])

(defn pairs [values]
  (loop [left values, right (next left), current []]
    (let [[lhead & ltail] left, [rhead & rtail] right]
      (if
        (not rhead) current
        (recur ltail rtail (conj current [lhead rhead]))))))

(defn increasing [values]
  (let [first<second (fn [[a b]] (< a b))]
    (count (filter first<second (pairs values)))))

(def actual-values
  (map edn/read-string (string/split-lines (slurp "../input.txt"))))

(println "Example count:" (increasing example-values))
(println "Actual count:" (increasing actual-values))
