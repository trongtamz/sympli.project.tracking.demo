import { EventEmitter, EventSubscription } from 'fbemitter';

interface ISubscriber {
  subscriber: any;
  subscription: EventSubscription;
}

const emitter = new EventEmitter();
let subscribers: Array<ISubscriber> = [];

export const publish = (eventName: string, message: any) => {
  emitter.emit(eventName, message);
};

export const subscribeToEvent = (eventName: string, callback: Function) => {
  return emitter.addListener(eventName, callback);
};

export const unsubscribeWithSubscription = (subscription: EventSubscription) => {
  if (subscription) {
    subscription.remove();
  }
};

export const subscribe = (subscriber: any, eventName: string, callback: Function) => {
  const subscription = subscribeToEvent(eventName, callback);

  subscribers.push({
    subscriber,
    subscription,
  });

  publish('eventbus.addedListener', {
    listeners: emitter.listeners.length,
  });
};

export const unsubscribe = (subscriber: any) => {
  const listeners = subscribers.filter((x) => x.subscriber === subscriber);

  subscribers = subscribers.filter((x: any) => x.subscriber !== subscriber);

  listeners.forEach((x) => x.subscription.remove());
};
