(ns punkislamist.core
  (:require [clojure.edn :as edn]))

(defn pwd [] (System/getProperty "user.dir"))


(defn to-number [s] (edn/read-string s))


(defn pivot [colls]
  (apply map vector colls))


(defn contained? [reference coll]
  (if (every? #(.contains reference %) coll)
    coll
    nil))


(defn take-upto
  "Similar to take-while, but returns items from coll
   that do not fulfill the pred, plus the first item
   that does fulfill it."
  ([pred coll]
   (lazy-seq
    (when-let [s (seq coll)]
      (if (pred (first s))
        (cons (first s) [])
        (cons (first s) (take-upto pred (rest s))))))))


(defn sum-range
  "Sum of all numbers between start and end."
  [start end]
  (let [n (+ 1 (- end start))]
    (quot (* n (+ start end)) 2)))
