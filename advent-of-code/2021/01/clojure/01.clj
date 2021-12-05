(require '[clojure.edn :as edn])
(require '[clojure.string :as string])


(def example-values [199 200 208 210 200 207 240 269 260 263])


(def actual-values
  (map edn/read-string (string/split-lines (slurp "../input.txt"))))


(defn windows [window-size values]
  (loop [remaining  values
         windows    []]
    (if (< (count remaining) window-size) windows
        (recur (next remaining)
               (conj windows (take window-size remaining))))))


(defn increasing [values]
  (let [first<second (fn [[a b]] (< a b))]
    (count (filter first<second values))))


(defn neighbours [values] (windows 2 values))


(defn triplet-sums [values]
  (let [sum       (partial apply +)
        triplets  (windows 3 values)]
    (neighbours (map sum triplets))))


(println "First example count:" (increasing (neighbours example-values)))
(println "First actual count:" (increasing (neighbours actual-values)))
(println "Second example count:" (increasing (triplet-sums example-values)))
(println "Second actual count:" (increasing (triplet-sums actual-values)))
