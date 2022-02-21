(ns advent-of-code.2021.03.clojure.03
  (:require [clojure.string :as string]
            [punkislamist.core :as pi]))


(defn to-binary [string]
  (->> string
       (map str)
       (map pi/to-number)))


(defn sum-columns [rows]
  (apply map + rows))


(defn gamma-rate [rows]
  (let [threshold (quot (count rows) 2)
        more-ones-than-zeroes? #(> % threshold)]
    (->> rows
         sum-columns
         (map more-ones-than-zeroes?)
         (map #(if % 1 0)))))


(defn gamma-to-epsilon-rate [gamma-rate]
  (let [invert #(if (= % 1) 0 1)]
    (map invert gamma-rate)))


(defn to-base10 [binary]
  (Integer/parseInt (apply str binary) 2))


(defn power-consumption [gamma-rate]
  (let [epsilon-rate (gamma-to-epsilon-rate gamma-rate)]
    (->> [gamma-rate epsilon-rate]
         (map to-base10)
         (apply *))))


(defn pivot [colls]
  (loop [pivoted '()
         colls (map seq colls)]
    (if (some nil? colls)
      (reverse pivoted) ; we conj a list so we append to the front which reverses the order
      (recur (conj pivoted (map first colls)) (map next colls)))))


(defn sorted-frequency-by [key-sorter value-sorter coll]
  (->> coll
       frequencies
       (map identity)
       (sort-by first key-sorter)
       (sort-by second value-sorter)))


(defn highest-frequency [key-sorter coll]
  (ffirst (sorted-frequency-by key-sorter > coll)))


(defn lowest-frequency [key-sorter coll]
  (ffirst (sorted-frequency-by key-sorter < coll)))


(defn rating [bit-fn colls]
  (loop [colls colls
         bit-pos 0]
    (let [columns (pivot colls)
          bit (bit-fn (nth columns bit-pos))
          remaining (filter #(= bit (nth % bit-pos)) colls)]
      (if (= (count remaining) 1)
        (first remaining)
        (recur remaining (inc bit-pos))))))


(defn oxygen-rating [colls] (rating (partial highest-frequency >) colls))


(defn scrubber-rating [colls] (rating (partial lowest-frequency <) colls))

(def input (string/split-lines (slurp "./apps/advent-of-code/2021/03/input.txt")))


;; solution part 1 = 2640986
(->> input
     (map to-binary)
     gamma-rate
     power-consumption)


;; solution part 2 = 6822109
(let [binaries (map to-binary input)
      oxygen (oxygen-rating binaries)
      scrubber (scrubber-rating binaries)]
  (->> [oxygen scrubber]
       (map to-base10)
       (apply *)))
