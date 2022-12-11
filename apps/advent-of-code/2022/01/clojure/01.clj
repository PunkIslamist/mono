(ns advent-of-code.2022.01.clojure.01
  (:require [clojure [string :as string] [edn :as edn]]))


(defn sum [coll] (reduce + coll))


(defn inventories [calories-list]
  (let [partitions (partition-by nil? calories-list)]
    (filter #(some? (first %)) partitions)))


(def calories-list
  (->> "./apps/advent-of-code/2022/01/input.txt"
       (slurp)
       (string/split-lines)
       (map edn/read-string)))



(def solution-1 ;; => 66306
  (->> calories-list
       (inventories)
       (map sum)
       (apply max)))


(comment)
