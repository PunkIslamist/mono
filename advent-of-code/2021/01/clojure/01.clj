(require '[clojure.edn :as edn])
(require '[clojure.string :as string])


(def example-values [199 200 208 210 200 207 240 269 260 263])


(defn pairs [values]
  (loop [left    values
         right   (next left)
         current []]
    (if (empty? right) current
        (recur (next left)
               (next right)
               (conj current [(first left) (first right)])))))

(defn increasing [values]
  (let [first<second (fn [[a b]] (< a b))]
    (count (filter first<second (pairs values)))))


(def actual-values
  (map edn/read-string (string/split-lines (slurp "../input.txt"))))


(println "Example count:" (increasing example-values))
(println "Actual count:" (increasing actual-values))
