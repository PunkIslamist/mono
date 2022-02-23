(ns advent-of-code.2021.04.clojure.04
  (:require [clojure [string :as string]
             [data :as data]]
            [punkislamist.core :as pi]))


(defn string->moves [s]
  (map pi/to-number (string/split s #",")))


(defn strings->board [ss]
  (let [split-at-spaces #(string/split % #"\s+")
        to-number       (partial map pi/to-number)
        rows&columns    #(hash-map :rows %
                                   :columns (pi/pivot %))]
    (->> ss
         (map split-at-spaces)
         (map to-number)
         rows&columns)))


(defn parse [lines]
  (let [not-separator? (partial not= [""])]
    (->> lines
         (partition-by string/blank?)
         (filter not-separator?)
         ((juxt
           (comp vec string->moves ffirst)
           (comp (partial map strings->board) rest)))
         (zipmap [:moves :boards]))))


(defn unmarked [moves board]
  (let [field-set (set (flatten (:rows board)))
        move-set  (set moves)]
    (first (data/diff field-set move-set))))


(defn not-in [reference coll]
  (filter (fn [n] (not (.contains reference n))) coll))


(defn pick-side 
  "Pretty much binary search plus removal (marking)
   of items when we have to go right."
  [moves rows&cols]
  (let [mid   (quot (count moves) 2)
        left  (subvec moves 0 mid)
        right (subvec moves mid)]
    (if (some (partial pi/contained? left) rows&cols)
      [left rows&cols]
      [right (map (partial not-in left) rows&cols)])))


(defn idx-required-moves [moves rows&cols]
  (loop [[moves rows&cols] [moves rows&cols]]
    (if (= (count moves) 1)
      (.end moves)
      (recur (pick-side moves rows&cols)))))


(defn indexed [{:keys [moves boards]}]
  (let [all-values (partial mapcat second)
        idx (comp (partial idx-required-moves moves) all-values)]
    (map (juxt idx identity) boards)))


(def parsed
  (->> "./apps/advent-of-code/2021/04/input.txt"
     (slurp)
     (string/split-lines)
     (map string/trim)
     (parse)
     ((juxt :moves
            (comp (partial sort-by first) indexed)))))


(defn compute-score [board-selector
                     [all-moves played-boards]]
  (let [[idx winner] (board-selector played-boards)
        required-moves (subvec all-moves 0 idx)]
    (->> winner
         (unmarked required-moves)
         (apply +)
         (* (last required-moves)))))

(def part-1 (compute-score first parsed)) ; => 16674
(def part-2 (compute-score last parsed))  ; => 7075