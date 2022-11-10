(ns snail-sort-clojure.snail-sort
  (:require [punkislamist.core :as pi]))

(defn rotate [vs]
  (reverse (pi/pivot vs)))


(def input-odd [[:a1 :a2 :a3]
                [:b1 :b2 :b3]
                [:c1 :c2 :c3]])


(def input-even [[:a1 :b1 :c1 :d1]
                 [:a2 :b2 :c2 :d2]
                 [:a3 :b3 :c3 :d3]
                 [:a4 :b4 :c4 :d4]])


(defn snail-sort [vs]
  (loop [[head & tail] vs
         snail         []]
    (if (empty? tail)
      (concat snail head)
      (recur
       (rotate tail)
       (concat snail head)))))


(defn x-deltas []
  (let [zeroes (->> (range) (map #(repeat % 0)))
        ones   (->> (range) (map #(repeat % (if (even? %)
                                              1
                                              -1))))]
    (apply concat (interleave ones zeroes))))


(defn y-deltas []
  (let [zeroes (->> (range) (map #(repeat % 0)))
        ones   (->> (range) (map #(repeat % (if (even? %)
                                              -1
                                              1))))]
    (apply concat (interleave zeroes ones))))


(defn snail-indices-odd [vs]
  (let [mid-index     (quot (count vs) 2)
        element-count (* (count vs) (count (first vs)))
        xs            (reductions + mid-index (x-deltas))
        ys            (reductions + mid-index (y-deltas))]
    (->> (map vector xs ys)
         (take element-count)
         (reverse))))


(defn smart-snail [vs]
  (map #(get-in vs %) (snail-indices-odd vs)))

(comment
  (smart-snail input-odd)
  (snail-sort input-odd))