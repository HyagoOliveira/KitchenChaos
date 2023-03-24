using UnityEngine;
using ActionCode.Audio;
using KitchenChaos.Items;
using KitchenChaos.Orders;
using System.Collections.Generic;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public sealed class OrderTicketGroup : MonoBehaviour
    {
        [SerializeField] private IngredientSettings ingredientSettings;
        [SerializeField] private OrderTicket ticketPrefab;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSourceDictionary failedSource;
        [SerializeField] private AudioSourceDictionary deliveredSource;

        private readonly Dictionary<Order, OrderTicket> tickets = new(10);

        private void Reset() => audioSource = GetComponent<AudioSource>();

        private void OnEnable()
        {
            OrderManager.Instance.Settings.OnOrderCreated += HandleOrderCreated;
            OrderManager.Instance.Settings.OnOrderRemoved += HandleOrderRemoved;
            OrderManager.Instance.Settings.OnOrderDelivered += HandleOrderDelivered;
        }

        private void OnDisable()
        {
            OrderManager.Instance.Settings.OnOrderCreated -= HandleOrderCreated;
            OrderManager.Instance.Settings.OnOrderRemoved -= HandleOrderRemoved;
            OrderManager.Instance.Settings.OnOrderDelivered -= HandleOrderDelivered;
        }

        private void HandleOrderCreated(Order order)
        {
            var ticket = Instantiate(ticketPrefab, transform);

            ticket.Initialize(order);

            PlayCreatedSound();
            tickets.Add(order, ticket);
        }

        private void HandleOrderRemoved(Order order)
        {
            tickets[order].Dispose(order);
            Destroy(tickets[order].gameObject);
            tickets.Remove(order);
        }

        private void HandleOrderDelivered(Order order) => PlayDeliveredSound();

        private void PlayCreatedSound() => audioSource.Play();
        private void PlayRemovedSound() => failedSource.PlayRandom();
        private void PlayDeliveredSound() => deliveredSource.PlayRandom();
    }
}