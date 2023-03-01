declare global {
  interface GroupedArray<T> {
    [key: string]: T[];
  }
  interface Array<T> {
    /**Returns the first item from the array.*/
    first(): T;
    /**Returns the last item from the array.*/
    last(): T;
    /**Removes the provided item from the array.*/
    remove(item: T): void;
    /**Removes all the items from the array that match that item (e.g. it will remove multiple instance of '1' if there
     * are several in that array.*/
    removeAll(item: T): void;
    /**Removes an array of items from the array.*/
    removeItems(items: T[]): void;
    /**Removes an array of items from the array, so that every instance matching them is removed.*/
    removeAllItems(items: T[]): void;
    /**Updates the array with the new array based on the property provided being different.*/
    update(array: Array<T>, property: string): void;
    /**Returns all the items in the array except the ones provided (if values exist more than once, all will be removed),
     * but does not modify the original array.*/
    except(items: T[]): T[];
    /**Returns all the items in the array except the ones provided (if values exist more than once, the item will only
     * be removed once), but does not modify the original array.*/
    exceptSingle(items: T[]): T[];
    /**Creates a disconnected copy of the array.*/
    copy(): T[];
    /**Pushes an array of items to the array.*/
    pushItems(items: T[]): void;
    /**Checks if there are any items in the collection.*/
    any(): boolean;
    /**Checks if there are no items in the collection.*/
    none(): boolean;
    /**Removes all items from the array.*/
    clear(): void;
    /**Checks if the item exists in the array.*/
    exists(item: T): boolean;
    /**Moves the item in the array one level down if possible. The item previously occupying that space will take the
     * original position of the moved item.*/
    moveDown(item: T): void;
    /**Moves the item in the array one level up if possible. The item previous occupying that space will take the
     * original position of the moved item.*/
    moveUp(item: T): void;
    /** @returns A new array that contains all items but the first and last ones. If there are only one or two items,
     * an empty array gets returned. */
    centralItems(): T[];
    /** Returns the max index of the array. In other words, the length - 1. */
    maxIndex: number;
    /** Groups the array by the provided property. */
    groupBy(property: string): GroupedArray<T>;
  }
}

Array.prototype.first = function<T>(): T {
  return this[0];
};
Array.prototype.last = function<T>(): T {
  return this[this.length - 1];
};
Array.prototype.remove = function<T>(item: T): void {
  this.splice(this.indexOf(item), 1);
};
Array.prototype.removeAll = function<T>(item: T): void {
  while(this.exists(item)) {
    this.remove(item);
  }
};
Array.prototype.removeItems = function<T>(items: T[]): void {
  for (let item of items) {
    this.remove(item);
  }
};
Array.prototype.removeAllItems = function<T>(items: T[]): void {
  for (let item of items) {
    while(this.exists(item)) {
      this.remove(item);
    }
  }
};
Array.prototype.update = function<T>(array: Array<T>, property: string): void {
  const itemsToRemove: T[] = [];
  const itemsToAdd: T[] = [];
  for (let item of this) {
    // @ts-ignore
    const existingItem = array.find(i => i[property] === item[property]);
    if (!existingItem) itemsToRemove.push(item); // Can't remove from array in for loop because it will break the loop.
  }
  for (let item of array) {
    // @ts-ignore
    const existingItem = this.find(i => i[property] == item[property]);
    if (!existingItem) itemsToAdd.push(item);
  }
  this.removeItems(itemsToRemove);
  this.pushItems(itemsToAdd);

  if (this.length === array.length) {
    for (let i = 0; i < array.length; i++) {
      // @ts-ignore
      if (this[i][property] !== array[i][property]) this[i] = array[i];
    }
  }

};
Array.prototype.except = function<T>(items: T[]): T[] {
  const copy = this.copy();
  for (let item of items) {
    // Continues to check for the value being present in the array. E.g. for when the array has several of the same values (e.g. [0, 1, 1, 1, 3])
    while(copy.exists(item)) {
      copy.remove(item);
    }
  }
  return copy;
};
Array.prototype.exceptSingle = function<T>(items: T[]): T[] {
  const copy = this.copy();
  for (let item of items) {
    copy.remove(item);
  }
  return copy;
};
Array.prototype.copy = function<T> (): T[] {
  return this.slice();
};
Array.prototype.pushItems = function<T>(items: T[]): void {
  for (let item of items) {
    this.push(item);
  }
};
Array.prototype.any = function<T>(): boolean {
  return this.length > 0;
};
Array.prototype.none = function<T>(): boolean {
  return this.length <= 0;
};
Array.prototype.clear = function<T>(): void {
  this.splice(0, this.length);
};
Array.prototype.exists = function<T>(item: T): boolean {
  return this.find(value => value === item) !== undefined
};
Array.prototype.moveDown = function<T>(item: T) {
  const itemIndex = this.indexOf(item);
  if (itemIndex < 0) return;
  const lowerItem = this[itemIndex + 1];
  if (lowerItem === undefined) return;
  this[itemIndex + 1] = item;
  this[itemIndex] = lowerItem;
};
Array.prototype.moveUp = function<T>(item: T) {
  const itemIndex = this.indexOf(item);
  if (itemIndex <= 0) return;
  const upperItem = this[itemIndex - 1];
  if (upperItem === undefined) return;
  this[itemIndex - 1] = item;
  this[itemIndex] = upperItem;
};
Array.prototype.centralItems = function<T>() {
  const newArray: T[] = [];
  for (let index = 1; index < this.length - 1; index++) {
    const item = this[index];
    newArray.push(item);
  }
  return newArray;
}
Object.defineProperty(Array.prototype, 'maxIndex', {
  get (this: Array<any>) {
    return this.length - 1;
  }
});

Array.prototype.groupBy = function<T>(this: T[], prop: keyof T) {
  return this.reduce((acc, curr) => {
    const key = curr[prop]
    // @ts-ignore
    if (!acc[key]) {
      // @ts-ignore
      acc[key] = []
    }
    // @ts-ignore
    acc[key].push(curr)
    return acc
  }, {} as GroupedArray<T>)
}

export{}
