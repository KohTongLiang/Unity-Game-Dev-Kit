
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace GameCore.UI
{
    public class ModalElement : VisualElement
    {
        string m_Status;
        public string status { get; set; }

        public ModalElement()
        {
            m_Status = String.Empty;
        }

        public new class UxmlFactory : UxmlFactory<ModalElement> {}

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Status = new UxmlStringAttributeDescription { name = "status" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                // Allow element to accept child of any type
                get
                {
                    yield return new UxmlChildElementDescription(typeof(VisualElement));
                }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((ModalElement)ve).status = m_Status.GetValueFromBag(bag, cc);
            }
        }
    }
}