(ns advent-of-code.2021.05.clojure.05
  (:require [clojure.string :as string]
            [punkislamist.core :as pi]
            [clojure.set :as set]))


(def segments->digit
  {#{\c \f} 1
   #{\a \c \f} 7
   #{\b \c \d \f} 4
   #{\a \c \d \e \g} 2
   #{\a \c \d \f \g} 3
   #{\a \b \d \f \g} 5
   #{\a \b \c \e \f \g} 0
   #{\a \b \d \e \f \g} 6
   #{\a \b \c \d \f \g} 9
   #{\a \b \c \d \e \f \g} 8})


(defn split2 [re s]
  (string/split s re))


(defn entry->signals&output [entry]
  (->> entry
       (split2 #"\|")
       (map string/trim)
       (map (partial split2 #" "))))


(defn count-simple [simple-nums entry]
  (->> entry
       (entry->signals&output)
       (second)
       (map count)
       (filter simple-nums)
       (count)))


(defn signals->patterns [signals]
  (-> (->> signals
           (map set)
           (group-by count))
      (update 2 first)
      (update 3 first)
      (update 4 first)
      (update 7 first)))


(defn once [& colls]
  (->> colls
       (map frequencies)
       (apply merge-with +)
       (filter (comp (partial = 1) second))
       (map first)))


(defn a [patterns _]
  {\a (set/difference (patterns 3) (patterns 2))})


(defn b [patterns _]
  {\b #{(some (patterns 4) (apply once (patterns 5)))}})


(defn d [patterns m]
  {\d (set/difference (patterns 4) (m \b) (patterns 2))})


(defn f [patterns m]
  (let [abd (apply set/union (vals m))]
    {\f (-> (filter (partial set/subset? abd) (patterns 5))
            (first)
            (set/intersection (patterns 2)))}))


(defn g [patterns m]
  (let [abdf (apply set/union (vals m))]
    {\g (-> (filter (partial set/subset? abdf) (patterns 5))
            (first)
            (set/difference abdf))}))


(defn c [patterns m]
  {\c (set/difference (patterns 2) (m \f))})


(defn e [patterns m]
  {\e (set/difference (patterns 7) (apply set/union (vals m)))})


(defn char-map [patterns]
  (->> (reduce
        (fn [m f] (merge m (f m)))
        {}
        (map (fn [f] (partial f patterns)) [a b d f g c e]))
       (pi/update-v first)
       (set/map-invert)))


(defn signal->digit [cmap signal]
  (->> signal
       (map cmap)
       (set)
       (segments->digit)))


(defn entry->number [entry]
  (let [[sigs out] (entry->signals&output entry)
        patterns   (signals->patterns sigs)
        mapping    (char-map patterns)]
    (->> out
         (map (partial signal->digit mapping))
         (apply str)
         (pi/to-number))))


(->> (string/split-lines (slurp "apps/advent-of-code/2021/08/input.txt"))
     (map (partial count-simple #{2 3 4 7}))
     (reduce +)) ; => 445

(->> (string/split-lines (slurp "apps/advent-of-code/2021/08/input.txt"))
     (map entry->number)
     (apply +)) ; => 1043101
