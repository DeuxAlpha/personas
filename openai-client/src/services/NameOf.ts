/**
 * Provides the string of available properties of the provided type.
 * @param prop - The property to get stringified.
 * @example
 * class NameOfTest {
 *   Value: string;
 * }
 * NameOfMember<NameOfTest>('Value');
 */
export function NameOfMember<T>(prop: Extract<keyof T, string>): string {
  return prop;
}

/**
 * Provides the name of a base type with type safety. Use NameOfMember for nested properties of a type.
 * @param type - The class/type to get stringified.
 * @example
 * class NameOfTest {}
 * NameOfType(NameOfTest);
 */
export function NameOfType(type: any): string {
  return type.name;
}

/**
 * Provides the name of an object with type safety. Only supports the root object.
 * @param {Object} object - The object to get stringified. Must be put in brackets (e.g. {someObject})
 * @example
 * const nameOfTest = {};
 * NameOfObject({nameOfTest}); // Notice the brackets.
 */
export function NameOfObject(object: object): string {
  return Object.keys(object)[0]
}
